using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using JetBrains.Annotations;

public class PropaneTank : MonoBehaviour, IDamageDetailed
{
    public static PropaneTank instance;
    public GameObject fireObject;
    public GameObject tankObject;

    public Vector3 handPlacementOffset = new Vector3(-1,5,-6);
    
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

    public void Damage(MonoBehaviour cause, RaycastHit hit)
    {
        Debug.Log("Propane got damaged");
        Destroy(gameObject);
    }

    public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, [CanBeNull] Collider collider)
    {
        Debug.Log("Propane got damaged but more complicated");
    }
    
    
}
