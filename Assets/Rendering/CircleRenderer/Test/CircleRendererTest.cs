using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CircleRendererTest : MonoBehaviour
{
    public int segments;
    public float radius;
    public float thickness;
    CircleRenderer circleRenderer;

    void Awake()
    {
        circleRenderer = GetComponent<CircleRenderer>();
    }

    public void Run()
    {
        circleRenderer = GetComponent<CircleRenderer>();
        circleRenderer.BuildCircle(segments, radius, thickness);

//        var circle = new Circle();
//        var points = circle.GetTangentPoints(segments);
//        points = circle.GetTangentIntersectionPoints(points);
//        foreach (var point in points)
//            Debug.Log(point);
    }

    void Update()
    {
        Run();
    }
}
