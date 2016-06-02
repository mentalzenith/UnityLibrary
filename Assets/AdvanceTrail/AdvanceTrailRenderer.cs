using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailRenderer : SingletonMonoManager<AdvanceTrailRenderer>
{
    public int maxPointPerBatch;
    public int batches;

    List<AdvanceTrailNode> nodes;
    List<AdvanceTrailNode> updatedNodes;
    Mesh[] meshes;
    int meshIndex;


    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        meshes = new Mesh[batches];
        meshIndex = 0;

        nodes = new List<AdvanceTrailNode>();
        updatedNodes = new List<AdvanceTrailNode>();
    }

    public static void CreateRenderer()
    {
        var go = new GameObject("[AdvanceTrailRenderer]");
        go.AddComponent<AdvanceTrailRenderer>();
    }

    void RegisterInternal(AdvanceTrailNode node)
    {
        nodes.Add(node);
    }

    void UpdatePointInternal(AdvanceTrailNode node)
    {
        updatedNodes.Add(node);
    }

    void LateUpdate()
    {
        foreach (var node in updatedNodes)
            BuildPoint(node.lastPoint);
    }

    void BuildPoint(AdvanceTrailPoint point)
    {

    }

    void ChangeBatch()
    {
        meshIndex = meshIndex + 1 < batches ? meshIndex + 1 : 0;
        vertices = new Vector3[maxPointPerBatch * 6];
        triangles = new int[maxPointPerBatch * 6];

    }

    //static shortcut method
    public static void Register(AdvanceTrailNode node)
    {
        Instance.RegisterInternal(node);
    }

    public static void UpdatePoint(AdvanceTrailNode node)
    {
        Instance.UpdatePointInternal(node);
    }
}
