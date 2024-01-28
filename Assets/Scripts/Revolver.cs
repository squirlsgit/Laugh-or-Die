using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Weapon
{
    public LayerMask shootableLayer;
    public Transform bulletShootingPoint;
    public override void Action()
    {
        Debug.Log("gun act");
        RaycastHit hit;
        Debug.DrawRay(bulletShootingPoint.transform.position, bulletShootingPoint.TransformDirection(Vector3.forward) * 100, Color.yellow);
        if (Physics.Raycast(bulletShootingPoint.transform.position, bulletShootingPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, shootableLayer, QueryTriggerInteraction.Collide))
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

    public override void Move()
    {
        base.Move();
        // Camera c = Camera.main;
        // transform.rotation = c.transform.rotation;
    }
}
