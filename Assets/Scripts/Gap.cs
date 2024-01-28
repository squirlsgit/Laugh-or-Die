using System.Collections;
using System.Collections.Generic;
using SFX;
using UnityEngine;

public class Gap : MonoBehaviour
{
    public Transform indicatorArrow;

    void Update()
    {
        OscillateArrow();
    }

    public void Hit()
    {
        Mee.instance.IncreaseLaughMeter("knifeDance", 0.05f);
        Deactivate();
        Player.instance.score += 1;
        StabbingGame.instance.level = Player.instance.ScoreToLevel(Player.instance.score);
        Debug.Log(StabbingGame.instance.level);
        Player.instance.scoreText.text = "score: " + Player.instance.score;
        SourcePlayerEvents.instance.InvokeEvent("stabHitTable");
    }

    void OscillateArrow()
    {
        Vector3 pos = indicatorArrow.position;
        indicatorArrow.position = new Vector3(pos.x, pos.y + Mathf.Cos(Time.time * 5) / 400f, pos.z);
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
