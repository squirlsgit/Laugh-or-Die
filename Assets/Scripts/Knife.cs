using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    
    public LayerMask segmentLayer;
    public LayerMask gapLayer;
    private Segment _targetedSegment;
    private Segment _previousRaycastedSegment;
    private Gap _targetGap;
    
    void Update()
    {
        SegmentDetection();
        GapDetection();
    }
    
    public override void Action() 
    {
        gameObject.SetChildLayers(LayerMask.NameToLayer("Knife"));
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
    
    public void GapDetection()
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
    
    public void SegmentDetection()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 20, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity,
                segmentLayer))
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
        }
        else
        {
            if (_previousRaycastedSegment)
            {
                _previousRaycastedSegment.UnhighlightAll();
                _targetedSegment = null;
            }
        }
    }
}
