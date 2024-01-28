using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFX;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    private Volume m_Volume;
    VolumeProfile profile => m_Volume.sharedProfile;

    private Vignette _vignette;

    private PaniniProjection _panini;
    // public Vignette Vignette => _vignette ??= (profile.TryGet<Vignette>(out var _vignette) ? _vignette : null);
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
    public float bloodLossRate = 0;

    public Image bloodBar;
    public float stabPromptingInterval;

    public TMP_Text surviveTimeText;
    public TMP_Text scoreText;

    public float surviveTime;
    public int score;

    public float reach = 2f;
    public LayerMask touchable;
    private bool healing => activeHand?.mode == "heal";

    private Stack<Gap> currentGapPattern;
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
        anim.Play("Grab Knife");
        bloodAmount = maxBloodAmount;
        debugText.text = "Joint left: " + SegmentCount;
        currentGapPattern = StabbingGame.instance.Level1PatternFactory();

        StartCoroutine(ConstantlyShowRandomGapToStab());
    }

    public float injuredBloodThreshold = 100f;
    public bool IsInjured => bloodBar.fillAmount < injuredBloodThreshold;
    private void Update()
    {
        bloodAmount -= bloodLossRate * Time.deltaTime;
        if (bloodAmount <= 0)
        {
            BleedOut();
        }
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

    public UnityEvent<string> OnDeath = new();
    public void BleedOut()
    {   
        Debug.Log("Gameover");
        OnDeath.Invoke("bleedOut");
        SourcePlayerEvents.instance.InvokeEvent("bleedOut");
        SceneM.instance.GameOver();
    }
    public void Hurt()
    {
        debugText.text = "Joint left: " + SegmentCount;
        bloodLossRate = maxSegmentCount - SegmentCount; // 32 is the total number of segments
    }
    
    IEnumerator ConstantlyShowRandomGapToStab()
    {
        while (true)
        {
            if (currentGapPattern.Count == 0)
            {
                currentGapPattern = StabbingGame.instance.GapPatternFactory();
            }
            Gap highlightGap = currentGapPattern.Pop();
            foreach (Gap gap in gaps)
            {
                gap.Deactivate();
            }
            highlightGap.Activate();
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
            
            PropaneTank pt = hit.collider.GetComponent<PropaneTank>();
            if (pt != null)
            {
                hand.Heal();
            }
            
            Debug.Log(hit.transform.gameObject);

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

    public int ScoreToLevel(int score)
    {
        if (score > 30)
        {
            return 3;
        }
        else if (score > 15)
        {
            return 2;
        }
        else
        {
            return 1;
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
