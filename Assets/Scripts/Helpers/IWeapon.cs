using UnityEngine;

public interface IWeapon
{
    // public Rigidbody rb
    // {
    //     get;
    // }
    public Transform transform
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