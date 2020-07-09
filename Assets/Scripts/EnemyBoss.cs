using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    private LaserBeam _laserBeam;

    [SerializeField]
    private bool _enemyIsDestroyed = false;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    [SerializeField]
    private Vector3[] _wayPoints;
    private int _currentWaypoint;
    private int _nextWayPoint = 1;
    private float _lastWaypointSwitchTime;
    private float _speed = 3f;


    [SerializeField]
    private GameObject _enemyFirePrefab;

    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {

        _wayPoints = new Vector3[4];
        _wayPoints[0] = new Vector3(0f, 8f, 0f);
        _wayPoints[1] = new Vector3(0f, 2f, 0f);
        _wayPoints[2] = new Vector3(-7f, 2f, 0f);
        _wayPoints[3] = new Vector3(7f, 2f, 0f);

        _lastWaypointSwitchTime = Time.time;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _laserBeam = GameObject.Find("BossLaserBeam").GetComponent<LaserBeam>();

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
            Debug.LogError("AudioSource is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition = _wayPoints[_currentWaypoint];
        Vector3 endPosition = _wayPoints[_nextWayPoint];
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / _speed;
        float currentTimeOnPath = Time.time - _lastWaypointSwitchTime;
        transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        

        if (transform.position.Equals(endPosition) && _enemyIsDestroyed == false)
        {
            switch (_nextWayPoint)
            {
                case 1:
                    _currentWaypoint = 1;
                    _nextWayPoint = 2;
                    break;
                case 2:
                    _currentWaypoint = 2;
                    _nextWayPoint = 3;
                    break;
                case 3:
                    _currentWaypoint = 3;
                    _nextWayPoint = 2;
                    break;
                default:
                    Debug.Log("Default Value");
                    break;
            }
            _lastWaypointSwitchTime = Time.time;
        }

        if (transform.position.y <= 3f && _player != null)
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

            _laserBeam.FireLaser();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                TakeDamage(50);
                player.Damage();
            }


        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                TakeDamage(10);
                _player.AddScore(20);
            }
    

        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                TakeDamage(20);
                _player.AddScore(20);
            }

        }
    }

    public void TakeDamage(int _damage)
    {
        Transform healthBarTransform = transform.Find("HealthBar");
        Transform hbBackground = transform.Find("HealthBarBackground");
        HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
        healthBar.currentHealth -= Mathf.Max(_damage, 0);
        
        if (healthBar.currentHealth <= 0)
        {
            Destroy(hbBackground.gameObject);
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
