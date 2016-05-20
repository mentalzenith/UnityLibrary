using UnityEngine;
using System.Collections;
using System;

public class CircleRenderer: MonoBehaviour
{
    Vector2[] tangentPoints;
    Vector2[] tangentIntersectPoints;

    public void BuildCircle(int segment,float thickness)
    {
        tangentPoints = new Vector2[segment];

        var startPoint = new Vector2(0, 1);
        float segmentAngle = 360f / segment;
        float segmentRadian = 2 * Mathf.PI / segment;

        for (int i = 0; i < segment; i++)
            tangentPoints[i] = startPoint.RotateCounterClockwise(segmentRadian * i);

        tangentIntersectPoints = new Vector2[segment];

        for (int i = 0; i < segment; i++)
            tangentIntersectPoints[i] = GetTangentIntersectionPoints(tangentPoints[i], tangentPoints[i + 1 == tangentPoints.Length ? 0 : i + 1]);

        foreach (var point in tangentIntersectPoints)
            Debug.DrawLine(point, new Vector3(point.x, point.y, 0.1f), Color.red);
    }

    void BuildCircleMesh(int segment,float thickness)
    {
        var vertices = new Vector3[segment * 9];
        int verticesIndex = 0;

        for (int i = 0; i < segment; i++)
        {
//            Vector2 innerTangentPoint = 
//
//            vertices[verticesIndex++] = 
        }
    }

    Vector2 GetTangentIntersectionPoints(Vector2 tangentPoint1, Vector2 tangentPoint2)
    {    

        var tangent1 = new Vector2(tangentPoint1.y, -tangentPoint1.x);
        var tangent2 = new Vector2(tangentPoint2.y, -tangentPoint2.x);

        Vector3 line1 = GetLine(tangentPoint1 - tangent1, tangentPoint1 + tangent1);
        Vector3 line2 = GetLine(tangentPoint2 - tangent2, tangentPoint2 + tangent2);

        return GetLineIntersectionPoint(line1, line2);
    }

    Vector3 GetLine(Vector2 point1, Vector2 point2)
    {
        Debug.DrawLine(point1, point2, Color.green);
        return new Vector3(point2.y - point1.y, point1.x - point2.x, point1.x * point2.y - point2.x * point1.y);
    }

    Vector2 GetLineIntersectionPoint(Vector3 line1, Vector3 line2)
    {
        float delta = line1.x * line2.y - line2.x * line1.y;
        if (delta == 0)
            throw new ArgumentException("Lines are parallel");

        float x = (line2.y * line1.z - line1.y * line2.z) / delta;
        float y = (line1.x * line2.z - line2.x * line1.z) / delta;
        return new Vector2(x, y);
    }
}

public class Circle
{
    public Vector2 center;
    public float radius;

    public Circle()
    {
    }
}

public static class Vector2Extension
{
    public static Vector2 RotateCounterClockwise(this Vector2 point, float degree)
    {
        return new Vector2(
            point.x * Mathf.Cos(degree) - point.y * Mathf.Sin(degree),
            point.x * Mathf.Sin(degree) + point.y * Mathf.Cos(degree));
    }
}