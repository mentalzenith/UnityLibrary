using UnityEngine;
using System.Collections;

public static class ProcedualPrimitives
{
    static Vector3[] vertices;
    static Vector2[] uvs;
    static int[] triangles;
    static int index;

    public static GameObject CreateCube()
    {
        var gameObject = new GameObject("Cube");
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = CreateCubeMesh();
        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        return gameObject;
    }

    public static Mesh CreateCubeMesh()
    {
        index = 0;
        vertices = new Vector3[36];
        triangles = new int[36];
        uvs = new Vector2[36];

        //bottom
        MakeFace(
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f), 
            new Vector3(-0.5f, -0.5f, 0.5f)
        );

        //top
        MakeFace(
            new Vector3(0.5f, 0.5f, 0.5f), //2
            new Vector3(0.5f, 0.5f, -0.5f), //3
            new Vector3(-0.5f, 0.5f, -0.5f),//4
            new Vector3(-0.5f, 0.5f, 0.5f) //1
        );

        //left
        MakeFace(
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f), 
            new Vector3(-0.5f, 0.5f, 0.5f), 
            new Vector3(-0.5f, 0.5f, -0.5f) 
        );

        //right
        MakeFace(
            new Vector3(0.5f, 0.5f, 0.5f), //2
            new Vector3(0.5f, -0.5f, 0.5f), //3
            new Vector3(0.5f, -0.5f, -0.5f),//4
            new Vector3(0.5f, 0.5f, -0.5f) //1
        );

        //front
        MakeFace(
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f), 
            new Vector3(0.5f, 0.5f, 0.5f), 
            new Vector3(-0.5f, 0.5f, 0.5f) 
        );

        //back
        MakeFace(
            new Vector3(0.5f, 0.5f, -0.5f), //2
            new Vector3(0.5f, -0.5f, -0.5f), //3
            new Vector3(-0.5f, -0.5f, -0.5f),//4
            new Vector3(-0.5f, 0.5f, -0.5f) //1
        );


        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;

    }

    static void MakeFace(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        uvs[index + 0] = new Vector2(0, 1);
        uvs[index + 1] = new Vector2(1, 1);
        uvs[index + 2] = new Vector2(1, 0);
        uvs[index + 3] = new Vector2(1, 0);
        uvs[index + 4] = new Vector2(0, 0);
        uvs[index + 5] = new Vector2(0, 1);

        MakeTriangle(p1, p2, p3);
        MakeTriangle(p3, p4, p1);
    }

    static void MakeTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        triangles[index] = index;
        triangles[index + 1] = index + 1;
        triangles[index + 2] = index + 2;


        vertices[index++] = p1;
        vertices[index++] = p2;
        vertices[index++] = p3;
    }
}
