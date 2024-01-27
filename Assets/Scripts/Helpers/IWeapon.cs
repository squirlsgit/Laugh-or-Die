using UnityEngine;

public interface IWeapon
{
    public Transform transform
    {
        get;
    }
    public Rigidbody rb
    {
        get;
    }

    public Vector3 holdOffsetPosition
    {
        get;
        set;
    }
    
    public Quaternion holdOffsetRotation
    {
        get;
        set;
    }
    public void Move();
    public void Drop();
    public void Grab();
    public void Action();
    public void Straighten();
}