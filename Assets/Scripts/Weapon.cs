using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // do we actually need draggable class?
    private bool _isDragging;
    private Rigidbody rb => GetComponent<Rigidbody>();
    public LayerMask detectPreviewLayer;

    private Segment _targetedSegment;
    private Segment _previousRaycastedSegment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectOverview();
    }

    public virtual void OnDrag()
    {
        transform.position = ItemMoveZone.instance.MouseToZonePos();
    }

    private void OnMouseDown() 
    {
        _isDragging = true;
        rb.isKinematic = true;
        Straighten();
    }

    private void OnMouseUp() 
    {
        _isDragging = false;
        rb.isKinematic = false;
        rb.velocity = new Vector3(0,-20,0);
        _targetedSegment.Fling();
    }

    private void OnMouseDrag() 
    {
        OnDrag();
    }

    private void Straighten()
    {
        transform.rotation = Quaternion.Euler(0, 90, -90);
    }

    public virtual void DetectOverview()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 20, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, detectPreviewLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            _targetedSegment = hit.transform.gameObject.GetComponent<Segment>();
            if (_targetedSegment != _previousRaycastedSegment)
            {
                if (_previousRaycastedSegment != null)
                {
                    _previousRaycastedSegment.UnhighlightAll();
                }
            }
            _targetedSegment.HighlightAll();
            _previousRaycastedSegment = _targetedSegment;
        } else {
            if (_previousRaycastedSegment)
            {
                _previousRaycastedSegment.UnhighlightAll();
            }
        }
    }
}
