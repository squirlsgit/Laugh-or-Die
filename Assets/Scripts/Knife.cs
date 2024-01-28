using System;
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

    private bool _stabbing;
    private Vector3 stabFrom;
    private Vector3 stabTo;
    
    private void Start()
    {
    }
    
    void UpdateStabbingAnimation()
    {
        if (_stabbing)
        {
            transform.position = Vector3.MoveTowards(transform.position, stabTo, Time.deltaTime * 30);
            if (Vector3.Distance(transform.position, stabTo) < 0.001f)
            {
                _stabbing = false;
            }
        }
    }

    void Update()
    {
        SegmentDetection();
        GapDetection();
        UpdateStabbingAnimation();
    }

    public override void Move()
    {
        if (!_stabbing)
        {
            base.Move();
        }
    }

    public override void Action()
    {
        _stabbing = true;
        stabFrom = transform.position;
        stabTo = transform.position - holdOffsetPosition / 2;
        Debug.Log(stabFrom + " " + stabTo);
        
        gameObject.SetChildLayers(LayerMask.NameToLayer("Knife"));
        //rb.isKinematic = false;
        rb.velocity = new Vector3(0,-20,0);
        if (_targetedSegment)
        {
            _targetedSegment.Fling();
        }
    
        if (_targetGap != null)
        {
            _targetGap.Hit();
        }
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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 100, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity,
                segmentLayer))
        {
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
