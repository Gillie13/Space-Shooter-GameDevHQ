using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarm : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    private SpawnManager _spawnManager;

    public bool _enemyIsDestroyed = false;

    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 200f;
    private float _movementSpeed = 3.0f;


    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _particleFirePrefab;


    // Start is called before the first frame update
    void Start()
    {
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

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is Null!");
        }


    }


    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (Vector2.down) - _rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, -transform.up).z;
        _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
        _rigidBody.velocity = -transform.up * _movementSpeed;

        if (transform.position.y <= -7.5f || transform.position.y >= 7.5f || transform.position.x <= -11f || transform.position.x >= 11f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
        }

        if ((transform.position.y >= -5f || transform.position.y <= 6f || transform.position.x >= -9f || transform.position.x <= 9f) && _player != null)
        {
            CanFire();
        }

    }

    private void CanFire()
    {
        if (Time.time > _canFire && _enemyIsDestroyed == false)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            Instantiate(_particleFirePrefab, transform.position, Quaternion.identity);
            
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
            EnemyDestroyed();


        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            EnemyDestroyed();

        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            EnemyDestroyed();
        }
    }

    private void EnemyDestroyed()
    {
        _enemyIsDestroyed = true;
        _spawnManager.OnEnemyDeath();
        _anim.SetTrigger("OnEnemyDeath");
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.3f);
    }

}
