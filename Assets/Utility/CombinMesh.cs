using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshCombiner
{
    public static Mesh CombineMesh(MeshFilter[] meshFilters)
    {
        var combines = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combines);
        return mesh;
    }

    public static Mesh CombineMeshWithVertexColor(GameObject rootGameObject)
    {
        var combines = GetCombinesWithVertexColor(rootGameObject);

        var mesh = new Mesh();
        mesh.CombineMeshes(combines.ToArray());
        return mesh;
    }

    static List<CombineInstance> GetCombinesWithVertexColor(GameObject gameObject, List<CombineInstance> combines = null)
    {
        return GetCombinesWithVertexColor(gameObject, gameObject.transform.worldToLocalMatrix, combines);
    }

    static List<CombineInstance> GetCombinesWithVertexColor(GameObject gameObject, Matrix4x4 rootTransform, List<CombineInstance> combines = null)
    {
        if (combines == null)
            combines = new  List<CombineInstance>();

        if (!gameObject.activeSelf)
            return combines;

        //go down
        foreach (Transform child in gameObject.transform)
            GetCombinesWithVertexColor(child.gameObject, gameObject.transform.localToWorldMatrix, combines);

        //this node
        var filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null)
            return combines;

        var mesh = CopyMeshWithoutColor(filter.mesh);                           

        var renderer = gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            if (renderer.material.HasProperty("_Color"))
                SetVertexColor(mesh, renderer.material.color);
            gameObject.SetActive(false);
        }
        
        combines.Add(new CombineInstance{ mesh = mesh, transform = gameObject.transform.localToWorldMatrix  });    
        return combines;
    }

    static Mesh CopyMeshWithoutColor(Mesh mesh)
    {
        var output = new Mesh();

        var vertices = new Vector3[mesh.vertices.Length];
        Array.Copy(mesh.vertices, vertices, mesh.vertices.Length);

        var uv = new Vector2[mesh.uv.Length];
        Array.Copy(mesh.uv, uv, mesh.uv.Length);

        var normals = new Vector3[mesh.normals.Length];
        Array.Copy(mesh.normals, normals, mesh.normals.Length);

        int triangleCount = mesh.triangles.Length;
        var triangles = new int[triangleCount];
        Array.Copy(mesh.triangles, triangles, triangleCount);

        output.vertices = vertices;
        output.normals = normals;
        output.uv = uv;
        output.triangles = triangles;
        output.bounds = mesh.bounds;

        return output;
    }

    static void SetVertexColor(Mesh mesh, Color color)
    {
        int verticesCount = mesh.vertices.Length;
        var colors = new Color[verticesCount];
        for (int i = 0; i < verticesCount; i++)
            colors[i] = color;
        
        mesh.colors = colors;
    }
}
