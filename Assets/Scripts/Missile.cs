using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 200f; 
    private float _movementSpeed = 6.0f;


    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
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

}
