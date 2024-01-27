using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SFX
{
    public class SourcePlayer : MonoBehaviour
    {
        public SoundLibrary library;
        public AudioSource source;
        [ShowInInspector]
        public Dictionary<string, GameObject> items = new();

        private void Start()
        {
            SourcePlayerEvents.instance.OnEvent.AddListener(Play);
        }

        public void Play(string ev, Vector3? pos = null)
        {
            TrySpawn(ev, pos);
            library.TryPlay(ev, source, pos.GetValueOrDefault(transform.position));
        }

        public void TrySpawn(string ev, Vector3? pos)
        {
            if (!pos.HasValue)
            {
                if (items.ContainsKey(ev)) Debug.LogError("TrySpawn should always have position!");
                return;
                
            }
            if (items.ContainsKey(ev))
            {
                Instantiate(
                    items[ev],
                    pos.GetValueOrDefault(Vector3.zero),
                    Quaternion.identity,
                    transform.root
                );
            }
        }
    }
}