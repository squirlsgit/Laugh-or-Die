using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    // do we actually need draggable class?
    private bool _isDragging;

    public Rigidbody rb
    {
        get => GetComponent<Rigidbody>();
    }
    
    [SerializeField]
    private Vector3 _holdOffsetPosition;
    
    public Vector3 holdOffsetPosition
    {
        get
        {
            return _holdOffsetPosition;
        }
        set
        {
            _holdOffsetPosition = value;
        }
    }
    
    [SerializeField]
    private Quaternion _holdOffsetRotation;
    
    public Quaternion holdOffsetRotation
    {
        get
        {
            return _holdOffsetRotation;
        }
        set
        {
            _holdOffsetRotation = value;
        }
    }

    public virtual void Action()
    {
        Debug.Log("a weapon performed action");
    }
    
    public virtual void Move()
    {
        transform.position = ItemMoveZone.instance.MouseToZonePos(_holdOffsetPosition);
    }

    public virtual void Drop()
    {
        Debug.Log("drop");
        rb.isKinematic = false;
        Player.instance.activeHand.Free();
        Player.instance.activeWeapon = null;
        rb.velocity = new Vector3(0,-5,0);
    }

    public virtual void Grab()
    {
        _isDragging = true;
        rb.isKinematic = true;
        gameObject.SetChildLayers(LayerMask.NameToLayer("Ignore Raycast"));
        Straighten();
    }

    public virtual void Straighten()
    {
        transform.rotation = _holdOffsetRotation;
    }
}
