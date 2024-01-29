using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    
    public class Highlight : MonoBehaviour
    {
        [ShowInInspector]
        private string emissiveColorProperty = "_EmissiveColor";
        public UnityEvent OnHighlight = new();
        public UnityEvent OnRemoveHighlight = new();
        [ColorUsage(hdr:true, showAlpha:true)]
        [ShowInInspector]
        public Color emission = Color.black;
        public Color color = Color.white;
        private List<MeshRenderer> _renderers;
        [ReadOnly]
        [ShowInInspector]
        private Dictionary<Material, Color> colorCache = new Dictionary<Material, Color>();
        
        [ReadOnly]
        [ShowInInspector]
        private Dictionary<Material, Color> emissiveCache = new Dictionary<Material, Color>();
        private List<MeshRenderer> renderers => _renderers ??= GetComponentsInChildren<MeshRenderer>().ToList();

        [Button]
        public void highlight()
        {
            if (Player.instance.activeWeapon != null)
            {
                return;
            }
            foreach(var renderer in renderers)
            {
                if (!renderer)
                {
                    continue;
                }
                foreach (var mat in renderer.materials)
                {
                    colorCache[mat] = mat.color;
                    mat.color = color;
                    emissiveCache[mat] = mat.GetColor(emissiveColorProperty);
                    mat.SetColor(emissiveColorProperty, emission);
                }
            }
            OnHighlight.Invoke();
        }

        [Button]
        public void dehighlight()
        {
            
            foreach (var renderer in renderers)
            {
                if (!renderer)
                {
                    continue;
                }
                foreach (var mat in renderer.materials)
                {
                    if (colorCache.ContainsKey(mat))
                    {
                        mat.color = colorCache[mat];
                    }

                    if (emissiveCache.ContainsKey(mat))
                    {
                        if (mat.HasColor(emissiveColorProperty))
                        {
                            
                            mat.SetColor(emissiveColorProperty, emissiveCache[mat]);
                        }
                    }
                }   
            }

            OnRemoveHighlight.Invoke();
        }

        private void Start()
        {
            
            foreach(var renderer in renderers)
            {
                if (!renderer)
                {
                    continue;
                }
                foreach (var mat in renderer.materials)
                {
                    colorCache[mat] = mat.color;
                    if (mat.HasColor(emissiveColorProperty)) {
                        
                        emissiveCache[mat] = mat.GetColor(emissiveColorProperty);
                    }
                }
            }
        }
    }
}