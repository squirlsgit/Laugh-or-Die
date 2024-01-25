using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Segment parent;
    public List<Segment> children;
    private Rigidbody rb => GetComponent<Rigidbody>();

    void Start()
    {
        
    }

    public List<Segment> GetAllSegments()
    {
        Stack<Segment> segmentStack = new();
        segmentStack.Push(this);
        List<Segment> segments = new();
        while (segmentStack.Count > 0)
        {
            Segment segment = segmentStack.Pop();
            segments.Add(segment);
            foreach(Segment childSegment in segment.children)
            {
                segmentStack.Push(childSegment);
            }
        }
        return segments;
    }

    public void Fling()
    {
        // Fling for 3 seconds then destroy
        if (!parent)
        {
            return;
        }
        parent.children.Remove(this);
        foreach(Segment segment in GetAllSegments())
        {
            parent.children.Remove(segment);
        }

        rb.isKinematic = false;
        rb.velocity = new Vector3(0,2,2);
        Player.instance.Hurt();
        Mee.instance.FullLaughMeter();

        StartCoroutine(SegmentDying());
    }

    public void HighlightAll()
    {
        foreach(Segment segment in GetAllSegments())
        {
            segment.Highlight();
        }
    }

    public void UnhighlightAll()
    {
        foreach(Segment segment in GetAllSegments())
        {
            segment.Unhighlight();
        }
    }

    public void Highlight()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void Unhighlight()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    IEnumerator SegmentDying()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
