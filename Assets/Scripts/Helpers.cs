
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static void SetChildLayers(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetChildLayers(child.gameObject, layer);
        }
    }   
    public static T Random<T>(this List<T> l)
    {
        if (l.Count == 0)
        {
            return default(T);
        }
        return l[Mathf.RoundToInt(UnityEngine.Random.value * (l.Count - 1))];
    }
}
