using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Weapon
{
    public LayerMask shootableLayer;
    public Transform bulletShootingPoint;
    public override void Action()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, bulletShootingPoint.TransformDirection(Vector3.up) * 100, Color.yellow);
        if (Physics.Raycast(transform.position, bulletShootingPoint.TransformDirection(Vector3.up), out hit, Mathf.Infinity, shootableLayer, QueryTriggerInteraction.Collide))
        {
            IDamageDetailed damagable = hit.transform.GetComponentInParent<IDamageDetailed>();
            if (damagable != null)
            {
                damagable.Damage(this, hit);
            }
        }
    }

    public void Reload()
    {
        Debug.Log("Reload gun");
    }
}
