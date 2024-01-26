
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
}
