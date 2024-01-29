using System.Collections;
using System.Collections.Generic;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// [AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class CameraController : MonoBehaviour
{

    public float maxSensitivityX = 0.1f;
    public float maxSensitivityY = 0.1f;
    public float maxLookRotation = 90f;
    float rotationX = 0F;
    float rotationY = 0F;
    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;
    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;
    public float frameCounter = 20;
    Quaternion targetRotation;

    public float minimumX = -90F;
    public float maximumX = 90F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    public void SidePan(InputAction.CallbackContext context)
    {
        var rot = context.ReadValue<Vector2>();
        Debug.Log("rotate " + rot);
        _TrackMouse(new Vector2(rot.y, rot.x));
        // PanMouseTarget(new Vector3(0f, 90f * rot.x, 0f), 1f, true);
    }
    [Button]
    void PanMouseTarget(Vector3 rot, float duration, bool force = true)
    {
        // StartCoroutine(Pan(rot, duration, force));
    }

    IEnumerator Pan(Vector3 rot, float duration, bool force)
    {
        var start = Time.time;
        Quaternion offset = Quaternion.Euler(rot.x, rot.y, rot.z);
        var newRot = targetRotation * Quaternion.Inverse(offset);
        var originalRotation = targetRotation;
        while (Time.time < duration + start)
        {
            var delta = (Time.time - start) / duration;
            targetRotation = Quaternion.Lerp(originalRotation, newRot, Mathf.Clamp(delta, 0, 1));
            if (force)
            {
                transform.localRotation = targetRotation;
                
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private static float UnwrapAngle(float angle)
    {
        if (angle >= 180)
        {
            return angle - 360;
        }
        else
        {
            return angle;
        }
    }

    [ReadOnly]
    [ShowInInspector]
    private Highlight highlighted;
    public void highlightItems()
    {
        
        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen

        // actual Ray
            
        // debug Ray
        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        Debug.DrawRay(ray.origin, ray.direction * Player.instance.reach, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Player.instance.reach, Player.instance.touchable))
        {
            var go = hit.collider.gameObject;
            if (hit.articulationBody)
            {
                go = hit.articulationBody.gameObject;
            }
            var h = go.GetComponent<Highlight>();
            if (h && h != highlighted)
            {
                highlighted = h;
                h.highlight();
            }

            if (h != highlighted && highlighted)
            {
                highlighted?.dehighlight();
                highlighted = null;
            }
        }
        else
        {
            highlighted?.dehighlight();
            highlighted = null;
        }
    }
    private void _TrackMouse(Vector2 pos)
    {
        highlightItems();
        //Resets the average rotation
        rotAverageY = 0f;
        rotAverageX = 0f;
        // Debug.Log("Track Mouse " + pos);
        var delta = transform.localRotation.eulerAngles - targetRotation.eulerAngles;
        var deltaX = UnwrapAngle(Mathf.Abs(delta.x));
        var deltaY = UnwrapAngle(Mathf.Abs(delta.y));
        // we want to map 300-360 to -60 to 0
        // we want to map 0 - 60 to 0 to 60
        // if angle > 180, we subtract 360
        // if angle < 180 we do nothing
        // On the left deltaY = 300 - 360
        // On the right deltaY = 0 - 60
        var angle = Mathf.Clamp(Quaternion.Angle(targetRotation, transform.localRotation) / maxLookRotation, 0f, 1f);
        var scale = 1 - angle;
        var xScale = 1f;
        var yScale = 1f;
        if (Mathf.Sign(deltaY) == Mathf.Sign(pos.x))
        {
            xScale = scale;
        }
        if (Mathf.Sign(deltaX) != Mathf.Sign(pos.y))
        {
            yScale = scale;
        }
        
        //Gets rotational input from the mouse
        rotationY += pos.y * maxSensitivityY * yScale;
        rotationX += pos.x * maxSensitivityX * xScale;
        
        //Clamp the rotation average to be within a specific value range
        rotAverageY = ClampAngle(rotationY, minimumY, maximumY);
        rotAverageX = ClampAngle(rotationX, minimumX, maximumX);

        //Get the rotation you will be at next as a Quaternion
        Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
        //
        // Quaternion newRotation = targetRotation * xQuaternion * yQuaternion;
        // float delta = Quaternion.Angle(transform.localRotation, targetRotation) - Quaternion.Angle(newRotation, targetRotation);
        //Rotate
        transform.localRotation = targetRotation * xQuaternion * yQuaternion;
    }
    public void TrackMouse(InputAction.CallbackContext context)
    {
        var pos = context.ReadValue<Vector2>().normalized;
        _TrackMouse(pos);
    }
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        targetRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle %= 360;
        return Mathf.Clamp(angle, min, max);
    }
    
}
