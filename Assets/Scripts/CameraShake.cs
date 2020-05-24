using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform transform;

    private float _duration = 0.0f;
    private float _strength = 0.3f;
    private float _remainingDuration = 1.0f;

    private Vector3 _cameraPosition;


    public void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable()
    {
        _cameraPosition = transform.localPosition;

    }


    // Update is called once per frame
    void Update()
    {
        if (_duration > 0.0f)
        {
            transform.localPosition = _cameraPosition + Random.insideUnitSphere * _strength;
            _duration -= Time.deltaTime * _remainingDuration;
        }
        else
        {
            _duration = 0.0f;
            transform.localPosition = _cameraPosition;
        }

    }

    public void StartShake()
    {
        _duration = 0.5f;
    }


}
