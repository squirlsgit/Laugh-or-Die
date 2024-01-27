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
    }
}