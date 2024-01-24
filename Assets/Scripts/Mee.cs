using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mee : MonoBehaviour
{
    public static Mee instance;
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
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
