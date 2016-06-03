﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailManager : SingletonMonoManager<AdvanceTrailManager>
{
    public int maxPointPerBatch = 100;
    public int batches = 3;

    AdvanceTrailRenderer[] renderers;
    List<AdvanceTrailNode> nodes;
    List<AdvanceTrailNode> updatedNodes;

    int batchIndex;

    AdvanceTrailRenderer currentBatch{ get { return renderers[batchIndex]; } }

    void Awake()
    {
        Init();
        CreateRenderObjects();
    }

    void CreateRenderObjects()
    {
        renderers = new AdvanceTrailRenderer[batches];
        var shader = Shader.Find("Unlit/TrailBillBoard");

        if(shader==null)
            Debug.LogError("Shader not found");

        var material = new Material(shader);

        for (int i = 0; i < batches; i++)
        {
            var trailRenderer = new GameObject("Renderer " + i).AddComponent<AdvanceTrailRenderer>();
            trailRenderer.gameObject.transform.SetParent(transform);
            trailRenderer.Init(maxPointPerBatch, material);
            renderers[i] = trailRenderer;
        }
    }

    public void Init()
    {
        nodes = new List<AdvanceTrailNode>();
        updatedNodes = new List<AdvanceTrailNode>();
        batchIndex = 0;
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
        {
            if (currentBatch.IsFull)
                NextBatch();
            currentBatch.BuildPoint(node.lastPoint);

        }
        updatedNodes.Clear();

        for (int i = 0; i < batches; i++)
            renderers[i].UpdateMesh();
    }

    void NextBatch()
    {
        batchIndex = batchIndex + 1 < batches ? batchIndex + 1 : 0;
        currentBatch.ResetMeshData();
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;
//        GUILayout.Label(verticesIndex.ToString());
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

public class LastUpdatedPoint
{
    //    AdvanceTrailPoint;
}