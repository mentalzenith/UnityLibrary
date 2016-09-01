using UnityEngine;
using System.Collections;
using BuildingSystem;

public class ScreenRayCaster
{
    public static RaycastInfo Cast(Vector2 screenPoint)
    {
        var ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return new RaycastInfo{ hit = hit, ray = ray };
    }
}

public class RaycastInfo
{
    public RaycastHit hit;
    public Ray ray;
}

public static class ScreenUtility
{
    public static Vector2 GetAttunatedScreenPoint(Vector3 worldPosition, out float attunation)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        var cameraDistance = Vector3.Distance(worldPosition, Camera.main.transform.position);
//        var frustumHeight = 2.0f * cameraDistance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
//        attunation = 1 / frustumHeight * Screen.height;

        attunation = (Screen.height / (Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2)) / cameraDistance;
        return screenPoint;
    }
}