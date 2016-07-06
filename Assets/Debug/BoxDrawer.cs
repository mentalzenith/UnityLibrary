using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxDrawer : SingletonMonoManager<BoxDrawer>
{
    Mesh mesh;
    Queue<Data> drawQueue;
    Material material;

    void Awake()
    {
        mesh = ProcedualPrimitives.CreateCubeMesh();
        drawQueue = new Queue<Data>();
        material = new Material(Shader.Find("Standard"));
    }

    public static void Draw(Vector3 center, Vector3 halfSize, Quaternion rotation)
    {
        var matrix = Matrix4x4.TRS(center, rotation, halfSize);
        Instance.drawQueue.Enqueue(new Data{ matrix = matrix });
    }

    void OnRenderObject()
    {
        material.SetPass(0);
        while (drawQueue.Count > 0)
            Graphics.DrawMeshNow(mesh, drawQueue.Dequeue().matrix);
    }

    class Data
    {
        public Matrix4x4 matrix;
    }
}
