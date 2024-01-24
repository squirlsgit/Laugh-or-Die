using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public TMP_Text debugText;
    public static Player instance;
    public Segment leftHand;
    public Segment rightHand;
    
    public int SegmentCount => leftHand.GetAllSegments().Count + rightHand.GetAllSegments().Count;
    
    public float maxBloodAmount = 500;
    public float bloodAmount;
    public float painAmount;
    // TODO: don't hardcode initial limb count
    public float bloodLossRate => 32 - SegmentCount;
    public float painAmountIncreaseRate => 0;

    public Image bloodBar;
    public Image painBar;
    
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
    
    private void Start()
    {
        bloodAmount = maxBloodAmount;
        debugText.text = "Joint left: " + SegmentCount;
    }

    private void Update()
    {
        bloodAmount -= bloodLossRate * Time.deltaTime;
        bloodBar.fillAmount = bloodAmount / maxBloodAmount;
    }

    public void Hurt()
    {
        string expression = ":|";
        if (SegmentCount >= 25)
        {
            expression = ">:(";
        }
        if (SegmentCount < 25)
        {
            expression = ":|";
        }
        if (SegmentCount < 15){
            expression = ":)";
        }
        if (SegmentCount < 5){
            expression = ":D";
        }
        debugText.text = "Joint left: " + SegmentCount + " Mee is like " + expression;
    }
    
}
