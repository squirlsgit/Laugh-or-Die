using System.Collections;
using System.Collections.Generic;
using SFX;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Segment rootSegment;
    public string name; // left or right
    public string mode; // idle, grabbing

    public Vector3 initialPosition;
    public Quaternion initialRotation;
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == "grabbing")
        {
            OnMove();
        }

        /// @TODO should be intuitive for player. blood loss should imply stump is bleeding somewhere. 
        if (mode == "heal")
        {
            if (!PropaneTank.instance.IsActive)
            {
                return;
            }
            if (Player.instance.bloodLossRate > 0 && rootSegment.GetAllSegments().Count < 16)
            {
                Player.instance.bloodLossRate -= Time.deltaTime * 2;
            }
        }
    }
    
    public virtual void OnMove()
    {
        transform.position = Player.instance.activeWeapon.transform.position;
    }

    public void Grab()
    {
        mode = "grabbing";
        gameObject.SetActive(false);
        transform.position = Player.instance.activeWeapon.transform.position;
        SourcePlayerEvents.instance.InvokeEvent("knifePickUp", transform.position, Vector3.up);
    }

    public void Free()
    {
        mode = "idle";
        gameObject.SetActive(true);
        Reset();
    }

    public void Heal()
    {
        mode = "heal";
        Player.instance.activeHand = this;
        PropaneTank pt = PropaneTank.instance;
        transform.localPosition = pt.transform.position + pt.handPlacementOffset;
        SourcePlayerEvents.instance.InvokeEvent("handOverFire", transform.position, Vector3.up, transform.gameObject);
        var bleeding = GetComponentsInChildren<handblood>();
        foreach (var thing in bleeding)
        {
            Destroy(thing.gameObject);
        }
        
    }

    public void Reset()
    {
        mode = "idle";
        rootSegment.UnhighlightAll();
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Player.instance.activeHand = null;
    }
}
