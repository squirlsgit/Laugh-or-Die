using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemMoveZone : MonoBehaviour
{
    public static ItemMoveZone instance;
    public Transform zone;

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

    /// <summary>
    /// Keeps held items safely within reach.
    /// </summary>
    /// <returns></returns>
    public float reachBuffer = 2f;

    public float reach => Player.instance.reach - reachBuffer;
    public Vector3 MouseToZonePos(Vector3 offset)
    {
        // fixed height
        // screen space x, y to world space x, z
        Vector2 mousePos = Mouse.current.position.value;
        
        Vector2 relativeMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen

        // actual Ray

        
        // debug Ray
        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        Debug.DrawRay(ray.origin, ray.direction * reach, Color.red);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit, reach))
        {
            pos = hit.point;
        }
        else
        {
            pos = ray.origin + ray.direction * reach;
        }

        return pos + offset;
    }
}
