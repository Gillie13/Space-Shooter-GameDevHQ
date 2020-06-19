using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 200f;
    private float _movementSpeed = 4.0f;

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


        _anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.LogError("Anim is NULL");
        }
    }

    private void Update()
    {
        if (_enemyIsDestroyed == false)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, -transform.up).z;
            _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = -transform.up * _movementSpeed;
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
        Destroy(this.gameObject, 2.8f);
    }

}
