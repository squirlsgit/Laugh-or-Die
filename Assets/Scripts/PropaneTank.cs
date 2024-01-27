using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using JetBrains.Annotations;

public class PropaneTank : MonoBehaviour, IDamageDetailed
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
