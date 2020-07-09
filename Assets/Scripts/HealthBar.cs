using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public float maxHealth = 1000;
    public float currentHealth = 1000;
    private float _originalScale;


    // Start is called before the first frame update
    void Start()
    {
        _originalScale = gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * _originalScale;
        gameObject.transform.localScale = tmpScale;

    }
}
