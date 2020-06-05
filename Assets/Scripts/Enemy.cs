using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private float _speed = 3.0f;
    private Player _player;
    private Laser _laser;
    private Animator _anim;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyFirePrefab;

    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    //Randomised movements variables
    private float _latestChangeDirectionTime;
    private readonly float _directionChangeTime = 2f;
    private float _enemyVelocity = 3f;
    private Vector2 _movementDirection;
    private Vector2 _movementPerSecond;

    private bool _enemyIsDestroyed = false;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null!");
        }

        //Randomised Movement
        _latestChangeDirectionTime = 0f;
        CalculateNewMovementVector();

        _anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.LogError("Anim is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Calculate the frequency of direction change
        if (Time.time - _latestChangeDirectionTime > _directionChangeTime && _enemyIsDestroyed == false)
        {
            _latestChangeDirectionTime = Time.time;
            CalculateNewMovementVector();
        }


        transform.position = new Vector2(transform.position.x + (_movementPerSecond.x * Time.deltaTime), transform.position.y + (_movementPerSecond.y * Time.deltaTime));

        if (Time.time > _canFire && _enemyIsDestroyed == false)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyFirePrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
        
    }

    void CalculateNewMovementVector()
    {
        _movementDirection = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0f)).normalized;
        _movementPerSecond = _movementDirection * _enemyVelocity;
        
        if (transform.position.y <= -7f || transform.position.y >= 8f || transform.position.x <= -10f || transform.position.x >= 10f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
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
            _enemyIsDestroyed = true;
            _anim.SetTrigger("OnEnemyDeath");
            //_speed = 2;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _enemyIsDestroyed = true;
            _anim.SetTrigger("OnEnemyDeath");
            //_speed = 2;
            _audioSource.Play();
            // stop firing when destroyed
            // Do not reposition if it leaves the game area
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _enemyIsDestroyed = true;
            _anim.SetTrigger("OnEnemyDeath");
            //_speed = 2;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }


}