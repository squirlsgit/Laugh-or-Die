using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mee : MonoBehaviour
{
    public static Mee instance;
    public Image laughMeter;
    public float rateOfLaughterDecrease;
    public float accelerationOfLaughterDecrease;
    private void Awake() 
    {         
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    void Update()
    {
        DeltaDecreaseLaughMeter();
        // TODO: use a log function to gradually diminish the rate of decrease?
        rateOfLaughterDecrease += accelerationOfLaughterDecrease; 
    }

    void DeltaDecreaseLaughMeter()
    {
        laughMeter.fillAmount -= Time.deltaTime * rateOfLaughterDecrease;
    }

    public void IncreaseLaughMeter(float amount)
    {
        laughMeter.fillAmount += amount;
    }

    public void FullLaughMeter()
    {
        laughMeter.fillAmount = 1;
    }
}
