using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CircleRendererTest : MonoBehaviour
{
    public int segments;
    CircleRenderer circleRenderer;

    void Awake()
    {
        circleRenderer = GetComponent<CircleRenderer>();
    }

    public void Run()
    {
        circleRenderer = GetComponent<CircleRenderer>();
        circleRenderer.BuildCircle(segments, 0.1f);

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
