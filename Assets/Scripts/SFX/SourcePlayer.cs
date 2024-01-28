using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace SFX
{
    public class SourcePlayer : SerializedMonoBehaviour
    {
        public SoundLibrary library;
        public AudioSource source;
        [ShowInInspector] public SerializedDictionary<string, GameObject> items = new();

        private void Start()
        {
            SourcePlayerEvents.instance.OnEvent.AddListener(Play);
        }

        public void Play(string ev, Vector3? pos = null, Vector3? look = null, GameObject p = null)
        {
            TrySpawn(ev, pos, look, p);
            library.TryPlay(ev, source, pos.GetValueOrDefault(transform.position));
        }

        public void TrySpawn(string ev, Vector3? pos = null, Vector3? dir = null, GameObject p = null)
        {
            if (!pos.HasValue)
            {
                if (items.ContainsKey(ev)) Debug.LogError("TrySpawn should always have position!");
                return;
                
            }
            if (items.ContainsKey(ev))
            {

                var look = Quaternion.LookRotation(dir.GetValueOrDefault(Vector3.up));
                Instantiate(
                    items[ev],
                    pos.GetValueOrDefault(Vector3.zero),
                    look,
                    p?.transform ?? transform.root
                );
            }
        }
    }
}