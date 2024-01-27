using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace SFX
{
    [CreateAssetMenu(fileName = "SoundLibrary", menuName = "SoundLibrary", order = 0)]
    public class SoundLibrary : SerializedScriptableObject
    {
        [ShowInInspector] public SerializedDictionary<string, AudioClip> clips = new();
        [Button]
        public void TryPlay(string ev, AudioSource source, Vector3? position = null)
        {
            if (clips.ContainsKey(ev) && clips[ev] != null)
            {
                if (position.HasValue)
                {
                    source.transform.position = position.Value;
                }
                if (source.isPlaying && source.clip != clips[ev])
                {
                    source.Stop();
                }
                source.clip = clips[ev];
                source.Play();
            }
        }
    }
}