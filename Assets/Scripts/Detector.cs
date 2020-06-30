using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyFirePrefab;

    private float _time = 0f;

    private GameObject _colliderHitResult;


    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "PowerUp" && _colliderHitResult != hit.transform.gameObject)
            {
                _colliderHitResult = hit.transform.gameObject;
                Shoot();
            }

        }

    }


    void Shoot()
    {

            GameObject enemyLaser = Instantiate(_enemyFirePrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
    }

}
