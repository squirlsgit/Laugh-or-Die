using System.Collections;
using System.Collections.Generic;
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
    }

    public void Free()
    {
        mode = "idle";
        gameObject.SetActive(true);
        Reset();
    }

    public void Reset()
    {
        mode = "idle";
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
