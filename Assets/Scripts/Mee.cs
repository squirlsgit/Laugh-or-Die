using System;
using System.Collections;
using System.Collections.Generic;
using SFX;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// trigger animations
/// - kill
/// - die
/// stateful animations
/// - mood
/// -- happy = 0
/// -- sad = 1
/// </summary>
public class Mee : MonoBehaviour, IDamageDetailed
{
    public GameObject gameOverScreen;
    
    public static Mee instance;
    public Image laughMeter;
    public float rateOfLaughterDecrease;
    public float accelerationOfLaughterDecrease;
    public float laughThreshold = 0.7f;
    public float happyThreshold = 0.4f;
    public enum MeeState
    {
        Happy,
        Sad,
    }

    public MeeState State
    {
        get
        {
            return (MeeState)_animator.GetInteger("mood");
        }
        set
        {
            _animator.SetInteger("mood", (int)value);
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
        SourcePlayerEvents.instance.OnEvent.AddListener(OnStrikeWall);
    }

    private void OnStrikeWall(string arg0, Vector3? arg1, Vector3? arg2, GameObject arg3)
    {
        if (arg0 == "stabWall" || arg0 == "shootWall")
        {
            laughMeter.fillAmount = Mathf.Clamp(laughMeter.fillAmount - displeasureFromBadBehavior, 0, 1);
        }

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
        }
    }

    void DeltaDecreaseLaughMeter()
    {
        laughMeter.fillAmount -= Time.deltaTime * rateOfLaughterDecrease;
    }

    public void IncreaseLaughMeter(float amount)
    {
        
        laughMeter.fillAmount += amount;
        if (State == MeeState.Sad && laughMeter.fillAmount > happyThreshold)
        {
            State = MeeState.Happy;
        }
        if (laughMeter.fillAmount >= laughThreshold)
        {
            _animator.SetTrigger("laugh");
        }
    }

    public void FullLaughMeter()
    {
        laughMeter.fillAmount = 1;
    }

    public void Damage(MonoBehaviour cause)
    {
        Debug.Log("MEE WAS STABBED IN THE HEAD!!!!");
        _animator.SetTrigger("die");
    }

    public void Damage(MonoBehaviour cause, RaycastHit hit)
    {
        Debug.Log("MEE GOT SHOT");
        _animator.SetTrigger("die");
    }

    public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, Collider collider)
    {
        Debug.Log("MEE GOT SHOT, BUT COMPLICATED");
        _animator.SetTrigger("die");
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
