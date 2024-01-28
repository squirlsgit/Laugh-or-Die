using System;
using UnityEngine;
using UnityEngine.Events;

namespace SFX
{
    public class SourcePlayerEvents : MonoBehaviour
    {
        /// <summary>
        ///
        /// Player or general
        /// shootWall
        /// shootTable
        /// shootPropane
        /// chopFlesh
        /// massiveChopFlesh
        /// bleedOut
        /// handOverFire **
        /// bloodDripping ** handled in script
        /// bunsenBurner -- in scene tied to fire on or off.
        /// stabHitTable
        /// knifePickUp
        ///
        ///
        /// Mee
        /// bleedOut
        /// bored
        /// bravoEncore
        /// deathScream
        /// handOverFire
        /// intro
        /// </summary>
        public static SourcePlayerEvents instance;
        public UnityEvent<string, Vector3?, Vector3?, GameObject> OnEvent = new(); 
        private void Awake()
        {
            instance = this;
        }

        public void InvokeEvent(string ev, Vector3 position, Vector3 dir, GameObject p)
        {
            OnEvent.Invoke(ev, position, dir, p);
        }
        public void InvokeEvent(string ev, Vector3 position, Vector3 dir)
        {
            OnEvent.Invoke(ev, position, dir, null);
        }
        public void InvokeEvent(string ev, Vector3 position)
        {
            OnEvent.Invoke(ev, position, null, null);
        }

        public void InvokeEvent(string ev)
        {
            OnEvent.Invoke(ev, null, null, null);
        }
    }
}