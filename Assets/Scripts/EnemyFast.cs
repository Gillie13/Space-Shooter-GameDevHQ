using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 200f;
    private float _movementSpeed = 5.0f;
    private float _downSpeed = 4.0f;

    //Shield
    [SerializeField]
    private GameObject _shieldOnPrefab;
    private bool _isShieldActive = true;

    private Player _player;

    private Animator _anim;
    private AudioSource _audioSource;

    private SpawnManager _spawnManager;

    public bool _enemyIsDestroyed = false;


    private void Start()
    {

        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody2D>();


        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null!");
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL!");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        transform.GetChild(0).gameObject.SetActive(true);

    }

    private void Update()
    {
        if (_enemyIsDestroyed == false && _player != null)
        {
            if (Vector2.Distance(transform.position, _target.position) > 5f)
            {
                Vector2 direction = (Vector2.down) - _rigidBody.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, -transform.up).z;
                _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
                _rigidBody.velocity = -transform.up * _downSpeed;
            }
            else if (Vector2.Distance(transform.position, _target.position) < 5f)
            {
                Vector2 direction = (Vector2)_target.position - _rigidBody.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, -transform.up).z;
                _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
                _rigidBody.velocity = -transform.up * _movementSpeed;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                transform.GetChild(0).gameObject.SetActive(false);
                _audioSource.Play();
            }
            else
            {
                EnemyDestroyed();

            }


        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                transform.GetChild(0).gameObject.SetActive(false);
                _audioSource.Play();
            }
            else
            {
                EnemyDestroyed();

            }

        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                transform.GetChild(0).gameObject.SetActive(false);
                _audioSource.Play();
            }
            else
            {
                EnemyDestroyed();

            }
        }
    }

    private void EnemyDestroyed()
    {
        _enemyIsDestroyed = true;
        _spawnManager.OnEnemyDeath();
        _anim.SetTrigger("OnEnemyDeath");
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }

}
