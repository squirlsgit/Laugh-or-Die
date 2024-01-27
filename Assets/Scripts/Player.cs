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
    public float bloodLossRate => 32 - SegmentCount;
    public float painAmountIncreaseRate => 0;

    public Image bloodBar;
    public Image painBar;

    public float stabPromptingInterval;

    public TMP_Text surviveTimeText;
    public TMP_Text scoreText;

    public float surviveTime;
    public float score;

    public float reach = 2f;
    public LayerMask touchable = LayerMask.GetMask("Knife");
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

    public void LeftHandPickupItem(InputAction.CallbackContext context)
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
            Weapon weapon = hit.collider.GetComponent<Weapon>();
            if (weapon != null)
            {
                activeHand = leftHand;
                activeWeapon = weapon;
                weapon.Grab();
                activeHand.Grab();
            }
        }
    }
    
    public void RightHandPickupItem(InputAction.CallbackContext context)
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
            Weapon weapon = hit.collider.GetComponent<Weapon>();
            if (weapon != null)
            {
                activeHand = rightHand;
                activeWeapon = weapon;
                weapon.Grab();
                activeHand.Grab();
            }
        }
    }

    public void MouseMoveItem()
    {
        if (activeWeapon != null)
        {
            activeWeapon.Move();
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
}
