using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using JetBrains.Annotations;
using SFX;

public class PropaneTank : MonoBehaviour, IDamageDetailed
{
    public static PropaneTank instance;
    public GameObject fireObject;
    
    public GameObject tankObject;
    public bool IsActive => fireObject.activeInHierarchy;
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
        SourcePlayerEvents.instance.InvokeEvent("shootPropane", hit.point, hit.normal, transform.gameObject);
        fireObject.SetActive(false);
    }
    public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, [CanBeNull] Collider collider)
    {
        Debug.Log("Propane got damaged but more complicated");
    }
    
    
}
