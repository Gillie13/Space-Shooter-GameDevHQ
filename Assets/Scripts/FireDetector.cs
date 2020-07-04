using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDetector : MonoBehaviour
{
    private EnemyDodger _enemyDodger;
    


    // Start is called before the first frame update
    void Start()
    {
        _enemyDodger = transform.parent.gameObject.GetComponent<EnemyDodger>();

        if (_enemyDodger == null)
        {
            Debug.LogError("EnemyDodge is NULL");
        }
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            return;
        }

        if (other.tag =="Laser" || other.tag == "Missile")
        {
            _enemyDodger.DodgeFire();
        }
        
    }
}
