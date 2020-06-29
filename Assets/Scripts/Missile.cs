using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform _target;
    private GameObject _closest = null;
    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 200f; 
    private float _movementSpeed = 6.0f;




    private void Start()
    {
        FindClosestEnemy();
        _target = _closest.transform;
        _rigidBody = GetComponent<Rigidbody2D>();

    }


    void FixedUpdate() 
    {
        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = transform.up * _movementSpeed;
            Destroy(this.gameObject, 3f);

        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                _closest = go;
                distance = curDistance;
            }
        }
        return _closest;
    }

}
