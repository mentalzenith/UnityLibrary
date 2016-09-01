using UnityEngine;
using System.Collections;

public static class TransformExtension
{
    public static void SetLayer(this GameObject gameObject, int layer)
    {
        gameObject.transform.SetLayer(layer);
    }

    public static void SetLayer(this Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
            SetLayer(child, layer);
    }
	
}
