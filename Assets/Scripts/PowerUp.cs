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



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Player")
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
                        player.RestoreLife();
                        break;
                    case 5:
                        player.MissilesActive();
                        break;
                    case 6:
                        player.NoAmmoPowerDown();
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
