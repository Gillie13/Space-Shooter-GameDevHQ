using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed =3.0f;
    [SerializeField]
    private int _powerUpId;

    [SerializeField]
    private AudioClip _clip;

    private Transform _player;
    private Rigidbody2D _rigidBody;
    private float _movementSpeed = 6.0f;
    private Vector2 _moveDirection;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody2D>();

        if (_player == null)
        {
            Debug.LogError("Player is Null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
  
        if (Input.GetKeyDown(KeyCode.C))
        {
            _moveDirection = (_player.transform.position - transform.position).normalized * _movementSpeed;
            _rigidBody.velocity = new Vector2(_moveDirection.x, _moveDirection.y);
        }
        else if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "EnemyLaser")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (_powerUpId)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.CollectAmmo();
                        break;
                    case 4:
                        player.NoAmmoPowerDown();
                        break;
                    case 5:
                        player.MissilesActive();
                        break;
                    case 6:
                        player.RestoreLife();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }

            }
            Destroy(this.gameObject);
        }
    }


}
