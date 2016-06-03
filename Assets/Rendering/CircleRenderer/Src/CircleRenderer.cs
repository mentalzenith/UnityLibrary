using UnityEngine;
using System.Collections;
using System;

public class CircleRenderer: MonoBehaviour
{
    Vector2[] tangentPoints;
    Vector2[] tangentIntersect;

    float segmentRadian;
    float segmentAngle;
    float radius;

    //Mesh
    Vector3[] vertices;
    Vector2[] uv;
    Vector2[] uv2;
    int[] triangles;
    int verticesIndex;
    float thickness;

    public void BuildCircle(int segment, float radius, float thickness)
    {
        if (segment < 4)
            segment = 4;

        this.radius = radius;

        tangentPoints = new Vector2[segment];

        var startPoint = new Vector2(0, radius);
        segmentAngle = 360f / segment;
        segmentRadian = 2 * Mathf.PI / segment;

        for (int i = 0; i < segment; i++)
            tangentPoints[i] = startPoint.RotateCounterClockwise(segmentRadian * i);

        tangentIntersect = new Vector2[segment];

        for (int i = 0; i < segment; i++)
            tangentIntersect[i] = GetTangentIntersectionPoints(tangentPoints[i], tangentPoints[i + 1 == tangentPoints.Length ? 0 : i + 1]);

        var innerTangent = GetInnerTangent(segment, segmentRadian, radius, thickness);

//        foreach (var point in tangentPoints)
//            Debug.DrawLine(point, new Vector3(point.x, point.y, 0.1f), Color.cyan);
//        
//        foreach (var point in innerTangent)
//            Debug.DrawLine(point, new Vector3(point.x, point.y, 0.1f), Color.yellow);
//        
//        foreach (var point in tangentIntersect)
//            Debug.DrawLine(point, new Vector3(point.x, point.y, 0.1f), Color.red);

        this.thickness = thickness;
        BuildCircleMesh(tangentIntersect, innerTangent);
    }

    Vector2[] GetInnerTangent(int segment, float segmentRadian, float radius, float thickness)
    {
        var innerTangent = new Vector2[segment];
        for (int i = 0; i < segment; i++)
        {
            Vector2 point = tangentPoints[i].RotateCounterClockwise(segmentRadian / 2f);
            innerTangent[i] = Vector2.Lerp(Vector2.zero, point, (radius - thickness) / radius);
        }
        return innerTangent;
    }

    void BuildCircleMesh(Vector2[] tangentIntersect, Vector2[] innerTangent)
    {
        int segment = tangentIntersect.Length;
        vertices = new Vector3[segment * 6];
        triangles = new int[segment * 6];
        uv = new Vector2[segment * 6];
        uv2 = new Vector2[segment * 6];
        verticesIndex = 0;

        for (int i = 0; i < segment; i++)
        {
            var next = i + 1 < segment ? i + 1 : 0;
            PushTriangle(innerTangent[i], tangentIntersect[next], tangentIntersect[i]);
            PushTriangle(innerTangent[i], innerTangent[next], tangentIntersect[next]);
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.uv2 = uv2;
        mesh.triangles = triangles;

        var filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        var renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = new Material(Shader.Find("Unlit/CircleRenderer"));
    }

    void PushTriangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        uv[verticesIndex] = ConvertToUV(p1);
        uv[verticesIndex + 1] = ConvertToUV(p2);
        uv[verticesIndex + 2] = ConvertToUV(p3);

        uv2[verticesIndex] = new Vector2(thickness / radius/2, 0);
        uv2[verticesIndex + 1] = new Vector2(thickness / radius/2, 0);
        uv2[verticesIndex + 2] = new Vector2(thickness / radius/2, 0);

        triangles[verticesIndex] = verticesIndex;
        triangles[verticesIndex + 1] = verticesIndex + 1;
        triangles[verticesIndex + 2] = verticesIndex + 2;

        vertices[verticesIndex++] = new Vector3(p1.x, p1.y, 0);
        vertices[verticesIndex++] = new Vector3(p2.x, p2.y, 0);
        vertices[verticesIndex++] = new Vector3(p3.x, p3.y, 0);
    }

    Vector2 ConvertToUV(Vector2 point)
    {
        point /= radius;
        return new Vector2(point.x * 0.5f + 0.5f, point.y * 0.5f + 0.5f);
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
    public static Vector2 RotateCounterClockwise(this Vector2 point, float radian)
    {
        return new Vector2(
            point.x * Mathf.Cos(radian) - point.y * Mathf.Sin(radian),
            point.x * Mathf.Sin(radian) + point.y * Mathf.Cos(radian));
    }
}