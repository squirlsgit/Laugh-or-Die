using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // TODO: Bryan code clean up items (Nick please add more that Bryan needs to clean up):
    // TODO: move all ui references to another script

    public Weapon activeWeapon;
    public Hand activeHand;
        
    public TMP_Text debugText;
    public static Player instance;
    public Hand leftHand;
    public Hand rightHand;
    
    public int SegmentCount => leftHand.rootSegment.GetAllSegments().Count + rightHand.rootSegment.GetAllSegments().Count;
    public List<Gap> gaps;
    
    public float maxBloodAmount = 500;
    public float bloodAmount;
    public float painAmount;
    // TODO: don't hardcode initial limb count
    public float bloodLossRate => 32 - SegmentCount;
    public float painAmountIncreaseRate => 0;

    public Image bloodBar;
    public Image painBar;

    public float stabPromptingInterval;

    public TMP_Text surviveTimeText;
    public TMP_Text scoreText;

    public float surviveTime;
    public float score;
    
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

        StartCoroutine(ConstantlyShowRandomGapToStab());
    }

    private void Update()
    {
        bloodAmount -= bloodLossRate * Time.deltaTime;
        bloodBar.fillAmount = bloodAmount / maxBloodAmount;
        
        surviveTime = Time.time;
        surviveTimeText.text = "survived: " + surviveTime.ToString("#.##");

        HandleMouseEvents();
        if (activeWeapon)
        {
            activeWeapon.OnMove();
        }
    }

    public void Hurt()
    {
        debugText.text = "Joint left: " + SegmentCount;
    }

    public void ShowRandomGapToStab()
    {
        int randomIndex = Random.Range(0, (gaps.Count - 1));
        foreach (Gap gap in gaps)
        {
            gap.Deactivate();
        }
        gaps[randomIndex].Activate();

    }

    IEnumerator ConstantlyShowRandomGapToStab()
    {
        while (true)
        {
            ShowRandomGapToStab();
            yield return new WaitForSeconds(stabPromptingInterval);
        }
    }

    public void HandleMouseEvents()
    {
        bool leftClicked = Input.GetMouseButtonDown(0);
        bool rightClicked = Input.GetMouseButtonDown(1);
        
        bool leftUp = Input.GetMouseButtonUp(0);
        bool rightUp = Input.GetMouseButtonUp(1);
        
        if (leftClicked || rightClicked)
        {
            // if (rightClicked && activeWeapon)
            // {
            //     activeWeapon.OnAction();
            // }
            
            if (leftClicked && activeWeapon)
            {
                activeWeapon.OnAction();
                return;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Weapon weapon = hit.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    activeHand = leftClicked ? leftHand : rightHand;
                    activeWeapon = weapon;
                    weapon.OnGrab();
                    activeHand.Grab();
                }
            }
        }
    }
}
