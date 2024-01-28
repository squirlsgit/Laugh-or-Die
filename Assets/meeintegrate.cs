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
