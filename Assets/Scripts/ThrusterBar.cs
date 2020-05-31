using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    private Image barImage;
    public const int thrusterMax = 100;
    public float _thrusterFuel = 0;
    private float _thrusterRefillAmount = 40f;   

    void Start()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    private void Update()
    {
        _thrusterFuel += _thrusterRefillAmount * Time.deltaTime;
        _thrusterFuel = Mathf.Clamp(_thrusterFuel, 0f, thrusterMax);
        

        barImage.fillAmount = GetThrusterNormalised();

    }

    public void UseThruster(int amount)
    {
        if(_thrusterFuel >= amount)
        {
            _thrusterFuel -= amount;
        }
    }

    public float GetThrusterNormalised()
    {
        return _thrusterFuel / thrusterMax;
    }
}