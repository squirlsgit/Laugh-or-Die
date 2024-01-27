using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // do we actually need draggable class?
    private bool _isDragging;
    private Rigidbody rb => GetComponent<Rigidbody>();
    public LayerMask segmentLayer;
    public LayerMask gapLayer;

    private Segment _targetedSegment;
    private Segment _previousRaycastedSegment;

    private Gap _targetGap;

    public bool isSelected;
    public bool isDoingWork;

    public Vector3 holdOffset = Vector3.up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SegmentDetection();
        GapDetection();
    }

    public virtual void Drag()
    {
        transform.position = ItemMoveZone.instance.MouseToZonePos(holdOffset);
    }
    
    public virtual void Move()
    {
        transform.position = ItemMoveZone.instance.MouseToZonePos(holdOffset);
    }

    public void Grab()
    {
        _isDragging = true;
        rb.isKinematic = true;
        gameObject.SetChildLayers(LayerMask.NameToLayer("Ignore Raycast"));
        Straighten();
    }
    
    public void Action() 
    {
        gameObject.SetChildLayers(LayerMask.NameToLayer("Knife"));
        _isDragging = false;
        rb.isKinematic = false;
        rb.velocity = new Vector3(0,-20,0);
        if (_targetedSegment)
        {
            _targetedSegment.Fling();
        }
    
        if (_targetGap != null)
        {
            _targetGap.Hit();
        }

        Player.instance.activeWeapon = null;
        Player.instance.activeHand.Free();
    }
    //
    // private void OnMouseDrag() 
    // {
    //     OnDrag();
    // }

    private void Straighten()
    {
        transform.rotation = Quaternion.Euler(0, 90, -90);
    }

    public virtual void GapDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, gapLayer))
        {
            Gap gap = hit.transform.gameObject.GetComponentInParent<Gap>();
            if (gap)
            {
                _targetGap = gap;
            }
        }
        else
        {
            _targetGap = null;
        }
    }

    public virtual void SegmentDetection()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 20, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity, segmentLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            _targetedSegment = hit.transform.gameObject.GetComponent<Segment>();
            if (!_targetedSegment)
            {
                return;
            }
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
                _targetedSegment = null;
            }
        }
    }
}
