using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 MouseToZonePos()
    {
        // fixed height
        // screen space x, y to world space x, z
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePosNormalized = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

        // Unity planes have 10x scale
        Vector3 pos = transform.position;
        Vector3 scale = transform.localScale;
        float leftBound = -scale.x * 10 + pos.x;
        float rightBound = scale.x * 10 + pos.x;
        float frontBound = -scale.z * 10 + pos.z;
        float backBound = scale.z * 10 + pos.z;
        
        float x = Mathf.Lerp(leftBound, rightBound, mousePosNormalized.x);
        float z = Mathf.Lerp(frontBound, backBound, mousePosNormalized.y);

        return new Vector3(x, transform.position.y, z);
    }
}
