using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldOnPrefab;
    [SerializeField]
    private GameObject _missileShotPrefab;
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldlives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    [SerializeField]
    private float _tripleShotCooldown = 5.0f;
    private bool _isMissilesActive = false;
    private float _missilesCooldown = 5.0f;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _SpeedBoostCooldown = 5.0f;
    [SerializeField]
    private bool _isShieldActive = false;
    private float _speedBoost = 1.0f;
    [SerializeField]
    private int _laserAmmo = 15;
    private int _laserAmmoMax = 15;
    private ThrusterBar _thrusters;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    private UI_Manager _uiManager;

    private CameraShake _shake;

    private SpriteRenderer _spriteRender;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _audioSource = GetComponent<AudioSource>();
        _shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _spriteRender = gameObject.transform.Find("Shields").gameObject.GetComponent<SpriteRenderer>();
        _thrusters = GameObject.Find("ThrusterBar").GetComponent<ThrusterBar>();


        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        if (_shake == null)
        {
            Debug.LogError("The Camera Shake is NULL!");
        }

        if (_spriteRender == null)
        {
            Debug.LogError("The SpriteRenderer is NULL!");
        }

        if (_thrusters == null)
        {
            Debug.LogError("The Thrusters is NULL!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        } else
        {
            _audioSource.clip = _laserSound;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
  
        CalculateMovement();
        float _thrusterFuel = _thrusters._thrusterFuel;



        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_thrusterFuel >= 2)
            {
                _speedBoost = 2.0f;
                _thrusters.UseThruster(1);
            }
            else
            {
                _speedBoost = 1.0f;
            }
        }
    }



    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * (_speed * _speedBoost) * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (_isMissilesActive == true)
        {
            _fireRate = 1.5f;
        } else
        {
            _fireRate = 0.2f;
        }

        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true && _isMissilesActive == false)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-0.2f, 0.15f, 0), Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound);
        }
        else if (_isMissilesActive ==true && _isTripleShotActive == false && GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            Instantiate(_missileShotPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        }
        else if (_laserAmmo >= 1 && _isMissilesActive == false)
        {
            _laserAmmo--;
            _uiManager.UpdateAmmo(_laserAmmo);
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound);
        } 

        
    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            
            _shieldlives--;
            if (_shieldlives == 2)
            {
                _spriteRender.color = new Color (1f, 1f, 1f, 0.6f);
                return;
            }
            else if (_shieldlives == 1)
            {
                _spriteRender.color = new Color (1f, 1f, 1f, 0.2f);
                return;
            }
            else
            {
                _isShieldActive = false;
                transform.GetChild(0).gameObject.SetActive(false);
                return;
            }
            
        }

        _shake.StartShake();

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
            _audioSource.PlayOneShot(_explosionSound);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
            _audioSource.PlayOneShot(_explosionSound);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);

        }

    }

    public void RestoreLife()
    {
        if (_lives == 2)
        {
            _lives++;
            _leftEngine.SetActive(false);

        } else if (_lives == 1)
        {
            _lives++;
            _rightEngine.SetActive(false);
        }

        _uiManager.UpdateLives(_lives);
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldlives = 3;
        transform.GetChild(0).gameObject.SetActive(true);
        _spriteRender.color = new Color(1f, 1f, 1f, 1f);

    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive == true)
        {
            yield return new WaitForSeconds(_tripleShotCooldown);
            _isTripleShotActive = false;
        }
    }

    public void MissilesActive()
    {
        _isMissilesActive = true;
        StartCoroutine(MissilesActiveRoutine());
    }

    IEnumerator MissilesActiveRoutine()
    {
        while(_isMissilesActive == true)
        {
            yield return new WaitForSeconds(_missilesCooldown);
            _isMissilesActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostActiveRoutine());
    }

    IEnumerator SpeedBoostActiveRoutine()
    {
        while (_isSpeedBoostActive == true)
        {
            _speed = 8.5f;

            yield return new WaitForSeconds(_SpeedBoostCooldown);
            _speed = 5.0f;
            _isSpeedBoostActive = false;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void CollectAmmo()
    {
        _laserAmmo += 15;
        if (_laserAmmo > 15) { 
            _laserAmmo = _laserAmmoMax;
        }
        _uiManager.UpdateAmmo(_laserAmmo);
    }

    public void NoAmmoPowerDown()
    {
        _laserAmmo = 0;
        _uiManager.UpdateAmmo(_laserAmmo);
    }

}
