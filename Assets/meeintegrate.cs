using System.Collections;
using System.Collections.Generic;
using SFX;
using UnityEngine;

public class meeintegrate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// @TODO add jother source to bypass reverb for intro
    /// </summary>
    public void PlayIntro()
    {
        SourcePlayerEvents.instance.InvokeEvent("intro");
    }

    public void MoveToOtherSpot()
    {
        Mee.instance.MoveToOtherSpot();
    }

    public void Die()
    {
        Mee.instance.Die();
        
    }
}
