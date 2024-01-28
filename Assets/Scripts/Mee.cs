using System;
using System.Collections;
using System.Collections.Generic;
using SFX;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// trigger animations
/// - kill
/// - die
/// - gift
/// - laugh
/// stateful animations
/// - mood
/// -- happy = 0
/// -- sad = 1
/// 
/// - standing
/// -- true
/// -- false
/// </summary>
public class Mee : MonoBehaviour, IDamageDetailed
{
    public Transform playerHead;
    public static Mee instance;
    public Image laughMeter;
    public float rateOfLaughterDecrease;
    public float accelerationOfLaughterDecrease;
    public float laughThreshold = 0.2f;
    public float happyThreshold = 0.4f;
    public float excitedThreshold = 0.85f;
    public float giftTimeThreshold = 60f;
    private List<Transform> positionsAfterGun = new();
    public enum MeeState
    {
        Happy,
        Sad,
        Excited,
        Gift,
    }

    public bool Standing
    {
        get
        {
            return _animator.GetBool("standing");
        }
        set
        {
            _animator.SetBool("standing", value);
        }
    }
    // public bool Interested
    // {
    //     get
    //     {
    //         return _animator.GetBool("standing");
    //     }
    //     set
    //     {
    //         _animator.SetBool("standing", value);
    //     }
    // }

    public MeeState State
    {
        get
        {
            return (MeeState)_animator.GetInteger("mood");
        }
        set
        {
            _animator.SetInteger("mood", (int)value);
            if (value == MeeState.Sad)
            {
                SourcePlayerEvents.instance.InvokeEvent("bored");
            }

            if (value == MeeState.Happy)
            {
                SourcePlayerEvents.instance.InvokeEvent("bravoEncore");
            }
            
            if (value == MeeState.Excited)
            {
                SourcePlayerEvents.instance.InvokeEvent("bravoEncore");
            }
            if (value == MeeState.Gift)
            {
                SourcePlayerEvents.instance.InvokeEvent("intro");
                _animator.SetTrigger("gift");
            }
        }
    }
    
    [SerializeField]
    private Animator _animator;

    public float timeThreshold = 20f;
    public float startTime;
    [Range(0,1)]
    public float displeasureFromBadBehavior = 0.5f;
    private void Awake()
    {
        startTime = Time.time;
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        }

        SourcePlayerEvents.instance.OnEvent.AddListener(OnStrikeProp);
    }

    private void OnStrikeProp(string arg0, Vector3? arg1, Vector3? arg2, GameObject arg3)
    {
        if (arg0 == "shootWall" || arg0 == "shootTable")
        {
            laughMeter.fillAmount = Mathf.Clamp(laughMeter.fillAmount - displeasureFromBadBehavior, 0, 1);
        }

        if (arg0 == "shootPropane")
        {
            laughMeter.fillAmount = Mathf.Clamp(laughMeter.fillAmount - displeasureFromBadBehavior * 0.3f, 0, 1);
        }
    }

    /// <summary>
    ///
    /// called at beginning of intro to be synced. then animation controller goes into idle
    /// </summary>
    public void PlayIntro()
    {
        SourcePlayerEvents.instance.InvokeEvent("intro");
    }
    public void GiftGun()
    {
        State = MeeState.Gift;
    }

    /// <summary>
    ///
    /// Called after gifting gun through animation trigger
    /// </summary>
    public void MoveToOtherSpot()
    {
        SceneM.instance.BlindPlayer();
        Standing = true;
        transform.position = positionsAfterGun.Random().position;
        transform.LookAt(Player.instance.transform);
    }

    void Update()
    {
        DeltaDecreaseLaughMeter();
        // TODO: use a log function to gradually diminish the rate of decrease?
        rateOfLaughterDecrease += accelerationOfLaughterDecrease;
        if (State == MeeState.Happy && laughMeter.fillAmount < happyThreshold)
        {
            State = MeeState.Sad;
        }
        if (laughMeter.fillAmount < 0.01f && Time.time - startTime > timeThreshold)
        {
            KillPlayer();
        } else if (Time.time - startTime > giftTimeThreshold && laughMeter.fillAmount > 0.5f)
        {
            GiftGun();
        }
    }

    void DeltaDecreaseLaughMeter()
    {
        laughMeter.fillAmount -= Time.deltaTime * rateOfLaughterDecrease;
    }

    public float happinessFromChopFlesh = 0.2f;
    public float happinessFromMassiveChopFlesh = 0.3f;

    public float deminishingReturnsFromRepetition = 1f;
    [ReadOnly]
    public List<string> actionTypes = new();
    public void IncreaseLaughMeter(string ev, float amount)
    {
        if (ev == "knifeDance")
        {
            amount *= (float)Math.Sqrt(StabbingGame.instance.level);
        }
        else
        {
            if (actionTypes.Count > 0 && actionTypes[actionTypes.Count - 1] == "knifeDance")
            {
                amount *= (float)Math.Sqrt(StabbingGame.instance.level);
            }
            else
            {
                amount /= deminishingReturnsFromRepetition * actionTypes.FindAll(t => t == ev).Count;
            }
        }
        
        if (amount >= laughThreshold)
        {
            _animator.SetTrigger("laugh");
        }
        actionTypes.Add(ev);
        laughMeter.fillAmount += amount;
        if (State == MeeState.Sad && laughMeter.fillAmount > happyThreshold)
        {
            State = MeeState.Happy;
        } else if (State != MeeState.Excited && laughMeter.fillAmount > excitedThreshold)
        {
            State = MeeState.Excited;
        }

        if (laughMeter.fillAmount > 1f && Player.instance.bloodAmount >= 100f)
        {
            SceneM.instance.WinGame();
        }
    }

    public void Damage(MonoBehaviour cause)
    {
        Debug.Log("MEE WAS STABBED IN THE HEAD!!!!");
        Die();
    }

    public void Damage(MonoBehaviour cause, RaycastHit hit)
    {
        Debug.Log("MEE GOT SHOT");
        Die();
    }

    public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, Collider collider)
    {
        Debug.Log("MEE GOT SHOT, BUT COMPLICATED");
        Die();
    }

    /// <summary>
    ///  Triggers death animation, which calls Dead through animation, and dead() wins the game.
    /// </summary>
    public void Die()
    {
        _animator.SetTrigger("die");
        SourcePlayerEvents.instance.InvokeEvent("deathScream");
    }
    public void Dead()
    {
        SceneM.instance.WinGame();
        // SceneM.instance.GameOver();
    }

    public void KillPlayer()
    {
        _animator.SetTrigger("kill");
        SourcePlayerEvents.instance.InvokeEvent("killPlayer", Player.instance.transform.position,Vector3.up,null);
        SceneM.instance.GameOver();
    }
}
