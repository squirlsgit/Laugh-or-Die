using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    // do we actually need draggable class?
    private bool _isDragging;
    public Vector3 dropLocalPosition;
    public Quaternion dropLocalRotation;
    
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
        Player.instance.activeHand.Free();
        Player.instance.activeWeapon = null;
        transform.localPosition = dropLocalPosition;
        transform.localRotation = dropLocalRotation;
    }

    public virtual void Grab()
    {
        _isDragging = true;
        gameObject.SetChildLayers(LayerMask.NameToLayer("Ignore Raycast"));
        Straighten();
    }

    public virtual void Straighten()
    {
        transform.rotation = _holdOffsetRotation;
    }
}
