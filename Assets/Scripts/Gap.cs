using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    public Transform indicatorArrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OscillateArrow();
    }

    void Highlight()
    {
        
    }

    public void Hit()
    {
        Mee.instance.IncreaseLaughMeter(0.05f);
        Deactivate();
        Player.instance.score += 1;
        Player.instance.scoreText.text = "score: " + Player.instance.score;
    }

    void OscillateArrow()
    {
        Vector3 pos = indicatorArrow.position;
        indicatorArrow.position = new Vector3(pos.x, pos.y + Mathf.Cos(Time.time * 5) / 100f, pos.z);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
}
