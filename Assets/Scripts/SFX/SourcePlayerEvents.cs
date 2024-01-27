using System;
using UnityEngine;
using UnityEngine.Events;

namespace SFX
{
    public class SourcePlayerEvents : MonoBehaviour
    {
        public static SourcePlayerEvents instance;
        public UnityEvent<string, Vector3?> OnEvent = new(); 
        private void Awake()
        {
            instance = this;
        }

        public void InvokeEvent(string ev, Vector3 position)
        {
            OnEvent.Invoke(ev, position);
        }

        public void InvokeEvent(string ev)
        {
            OnEvent.Invoke(ev, null);
        }
    }
}