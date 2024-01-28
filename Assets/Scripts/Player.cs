using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // TODO: Bryan code clean up items (Nick please add more that Bryan needs to clean up):
    // TODO: move all ui references to another script

    public int maxSegmentCount = 32;

    public IWeapon activeWeapon;
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
    public float bloodLossRate = 0;
    public float painAmountIncreaseRate => 0;

    public Image bloodBar;
    public Image painBar;

    public float stabPromptingInterval;

    public TMP_Text surviveTimeText;
    public TMP_Text scoreText;

    public float surviveTime;
    public float score;

    public float reach = 2f;
    public LayerMask touchable;
    private bool healing => activeHand?.mode == "heal";
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

        MouseMoveItem();
        
        // Don't know how to use the inspector settings so have to do it here :(
        if (Mouse.current.rightButton.wasReleasedThisFrame || Mouse.current.leftButton.wasReleasedThisFrame)
        {
            DropActiveItem();
            StopHealing();
        }
    }

    public void Hurt()
    {
        debugText.text = "Joint left: " + SegmentCount;
        bloodLossRate = maxSegmentCount - SegmentCount; // 32 is the total number of segments
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
    
    

    public void HandClick(InputAction.CallbackContext context, Hand hand)
    {
        if (context.canceled)
        {
            return;
        }
        if (activeWeapon != null)
        {
            return;
        }
        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); 
        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        Debug.DrawRay(ray.origin, ray.direction * reach, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, reach, touchable))
        {
            Weapon weapon = hit.collider.GetComponentInParent<Weapon>();
            if (weapon != null)
            {
                activeHand = hand;
                activeWeapon = weapon;
                weapon.Grab();
                activeHand.Grab();
            }

            PropaneTank pt = hit.collider.GetComponentInParent<PropaneTank>();
            if (pt != null)
            {
                hand.Heal();
            }
        }
    }

    public void RightHandClick(InputAction.CallbackContext context)
    {
        HandClick(context, rightHand);
    }
    
    public void LeftHandClick(InputAction.CallbackContext context)
    {
        HandClick(context, leftHand);
    }

    public void MouseMoveItem()
    {
        if (activeWeapon != null)
        {
            activeWeapon.Move();
        }
    }

    public void StopHealing()
    {
        if (healing)
        {
            activeHand.Reset();
        }
    }

    public void DropActiveItem()
    {
        if (activeWeapon != null)
        {
            activeWeapon.Drop();
        }
    }
    
    public void ActionWithActiveWeapon(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            return;
        }
        if (activeWeapon != null)
        {
            activeWeapon.Action();
        }
    }

    public void ReloadRevolver()
    {
        if (activeWeapon is Revolver)
        {
            Revolver revolver = (Revolver)activeWeapon;
            revolver.Reload();
        }
    }
}
