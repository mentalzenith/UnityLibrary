using UnityEngine;
using System.Collections;

public class BoundsUtility
{
    /// <summary>
    /// Centers the object in local space
    /// </summary>
    /// <returns>The object</returns>
    /// <param name="o">GameObject with MeshFilter</param>
    public static float CenterObject(GameObject o)
    {
        float longestLength = 1f;
        Vector3 center = Vector3.zero;
		
        var bounds = new Bounds();
        var filters = o.GetComponentsInChildren<MeshFilter>();
        foreach (var filter in filters)
            bounds.Encapsulate(filter.mesh.bounds);

        longestLength = GetLongestLength(bounds);
        center = bounds.center;

        o.transform.localPosition = -center;
        return longestLength;
    }

    public static float GetLongestLength(Bounds bounds)
    {
        var size = bounds.size;
        float longest = size.x;
        if (size.y > longest)
            longest = size.y;
        if (size.z > longest)
            longest = size.z;
        return longest;
    }

    public static float GetModelLongestLength(GameObject o)
    {
        return GetLongestLength(GetModelBounds(o));
    }

    public static float GetModelHeight(GameObject o)
    {
        return GetModelBounds(o).size.y;
    }

    public static float GetMeshVolume(GameObject o)
    {
        var bounds = GetMeshBounds(o);
        return bounds.size.x * bounds.size.y * bounds.size.z;
    }

    public static Bounds GetMeshBounds(GameObject o)
    {
        var bounds = new Bounds();
        var filters = o.GetComponentsInChildren<MeshFilter>();
        foreach (var filter in filters)
            bounds.Encapsulate(filter.mesh.bounds);
        return bounds;
    }

    public static Bounds GetModelBounds(GameObject o)
    {
        var bounds = new Bounds();
        var renderers = o.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
            bounds.Encapsulate(renderer.bounds);
        return bounds;
    }
}
