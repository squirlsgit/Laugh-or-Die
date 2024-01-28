using System;
using UnityEngine;

/// <summary>
/// Use to play drip effect when player running out of health.
/// </summary>
    public class manageBleedDrip : MonoBehaviour
    {
        public float bloodThreshold = 0.5f;
        public AudioSource bloodDrip;
        private void Update()
        {
            if (Player.instance.bloodAmount <= bloodThreshold * Player.instance.maxBloodAmount && Player.instance.bloodLossRate > 0f)
            {
                if (!bloodDrip.isPlaying)
                {
                    bloodDrip.Play();
                }
            }
            else
            {
                bloodDrip.Pause();
            }
        }
    }