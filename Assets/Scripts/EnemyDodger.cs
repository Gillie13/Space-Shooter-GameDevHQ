using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodger : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    private bool _dodge = false;

    private float _speed = 2f;
    private float _dodgeDirection;

    private bool _enemyIsDestroyed = false;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private float _durationOfDodge;
    private float _dodgeSpeed;

    [SerializeField]
    private GameObject _enemyFirePrefab;

    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1f;


    // Start is called before the first frame update
    void Start()
    {
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
            Debug.LogError("Animinator is NULL");
        }


    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if (_dodge == true)
        {
            StartCoroutine(Dodge());
        }

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

    public void DodgeFire()
    {
        _dodge = true;
        _dodgeDirection = Random.Range(0f, 2f);
    }

 

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Detector")
        {
            return;
        }

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

    IEnumerator Dodge()
    {
        while (_dodge == true)
        {
            _durationOfDodge = Random.Range(2f, 3f);
            _dodgeSpeed = Random.Range(3f, 4f);
            
            if (_dodgeDirection <= 1)
            {
                _anim.SetTrigger("Dodge_fire");
                transform.position += Vector3.right * _dodgeSpeed * Time.deltaTime * _durationOfDodge;
                yield return new WaitForSeconds(0.28f);
                _anim.ResetTrigger("Dodge_fire");
                _dodge = false;

            }
            else
            {
                _anim.SetTrigger("DodgeLeft");
                transform.position -= transform.right * _dodgeSpeed * Time.deltaTime * _durationOfDodge;
                yield return new WaitForSeconds(0.28f);
                _anim.ResetTrigger("DodgeLeft");
                _dodge = false;

            }

        }

    }

}
