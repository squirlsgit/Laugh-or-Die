using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RotatePlaneToLookAtPlayer : MonoBehaviour
    {
        private void Update()
        {
            Rotate();
        }

        void Rotate()
        {
            var targetDirection = Player.instance.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        }
    }
}