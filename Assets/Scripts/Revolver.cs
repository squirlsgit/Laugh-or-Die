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
        Debug.DrawRay(bulletShootingPoint.transform.position, bulletShootingPoint.TransformDirection(Vector3.back) * 100, Color.yellow);
        if (Physics.Raycast(bulletShootingPoint.transform.position, bulletShootingPoint.TransformDirection(Vector3.back), out hit, Mathf.Infinity, shootableLayer, QueryTriggerInteraction.Collide))
        {
            Debug.Log(hit.transform.gameObject);
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

    public IEnumerator WaitUntilGunShowsUp()
    {
        yield return new WaitForSeconds(6);
        transform.gameObject.SetActive(true);
    }

    public override void Move()
    {
        base.Move();
        // Camera c = Camera.main;
        // transform.rotation = c.transform.rotation;
    }
}
