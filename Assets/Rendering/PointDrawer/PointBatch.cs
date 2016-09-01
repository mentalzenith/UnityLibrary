using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointBatch : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    //double buffer
    List<PointInfo> points1;
    List<PointInfo> points2;
    bool flip;

    public bool IsFull
    {
        get
        { 
            var points = flip ? points1 : points2;
            return points.Count >= 65535;
        }
    }

    void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
//        meshRenderer.material = new Material(Shader.Find("Hidden/PointSprite"));
//        meshRenderer.material = new Material(Shader.Find("Hidden/PointSpriteProcedual"));
        meshRenderer.material = Resources.Load("PointSprite", typeof(Material)) as Material;
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        points1 = new List<PointInfo>();
        points2 = new List<PointInfo>();
    }

    public void AddWorldPoint(PointInfo point)
    {
        var points = flip ? points1 : points2;
        points.Add(point);
    }

    void LateUpdate()
    {
        var points = flip ? points1 : points2;
        flip = !flip;

        if (points.Count == 0)
            return;

        var vertices = new Vector3[points.Count];
        var indexes = new int[points.Count];
        var colors = new Color[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            vertices[i] = points[i].position;
            indexes[i] = i;
            colors[i] = points[i].color;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(indexes, MeshTopology.Points, 0);
        mesh.colors = colors;

        points.Clear();
        SetCameraParam();
    }

    void SetCameraParam()
    {
        var param = Screen.height / (Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2);
        meshRenderer.material.SetFloat("_CameraParam", param);
    }

    public class PointInfo
    {
        public Vector3 position;
        public Color color;
        public float size;
    }
}
