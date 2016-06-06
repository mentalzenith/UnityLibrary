using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailManager : SingletonMonoManager<AdvanceTrailManager>
{
    public int maxPointPerBatch = 300;
    public int maxBatches;
    int batchCount;

    Queue<AdvanceTrailRenderer> batchQueue;
    List<AdvanceTrailRenderer> visibleBatches;
    List<AdvanceTrailNode> nodes;
    List<AdvanceTrailNode> newPointNodes;
    List<AdvanceTrailNode> updatePointNodes;

    int batchIndex;
    int activeBatchCount;

    Shader shader;
    Material material;

    AdvanceTrailRenderer currentBatch;

    void Awake()
    {
        Init();
        NextBatch();
    }

    AdvanceTrailRenderer CreateNewBatch()
    {
        var trailRenderer = new GameObject("Renderer ").AddComponent<AdvanceTrailRenderer>();
        trailRenderer.gameObject.transform.SetParent(transform);
        trailRenderer.Init(maxPointPerBatch, material);
        batchCount++;
        return trailRenderer;
    }

    public void Init()
    {
        nodes = new List<AdvanceTrailNode>();
        newPointNodes = new List<AdvanceTrailNode>();
        updatePointNodes = new List<AdvanceTrailNode>();
        visibleBatches = new List<AdvanceTrailRenderer>();
        batchQueue = new Queue<AdvanceTrailRenderer>();

        shader = Shader.Find("Unlit/TrailBillBoard");
        if (shader == null)
            Debug.LogError("Shader not found");
        material = new Material(shader);
    }

    void RegisterInternal(AdvanceTrailNode node)
    {
        nodes.Add(node);
    }

    void NewPointInternal(AdvanceTrailNode node)
    {
        newPointNodes.Add(node);
    }

    void UpdatePointInternal(AdvanceTrailNode node)
    {
        updatePointNodes.Add(node);
    }

    void LateUpdate()
    {
        foreach (var node in updatePointNodes)
            node.point.UpdatePoint();
        updatePointNodes.Clear();

        foreach (var node in newPointNodes)
        {
            if (currentBatch.IsFull)
                NextBatch();
            currentBatch.BuildPoint(node.point);
        }
        newPointNodes.Clear();

        activeBatchCount = 0;
        for (int i = 0; i < visibleBatches.Count; i++)
        {
            if (visibleBatches[i].UpdateMesh())
                activeBatchCount++;
            if (visibleBatches[i].IsExpired)
                RecycleBatch(visibleBatches[i]);
        }
    }

    void RecycleBatch(AdvanceTrailRenderer batch)
    {
        visibleBatches.Remove(batch);
        batch.gameObject.SetActive(false);
        batchQueue.Enqueue(batch);
    }

    void NextBatch()
    {
        if (batchQueue.Count > 0)
        {
            currentBatch = batchQueue.Dequeue();
            currentBatch.gameObject.SetActive(true);
        }
        else
            currentBatch = CreateNewBatch();

        visibleBatches.Add(currentBatch);
        currentBatch.ResetMeshData();
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;
        GUILayout.Label(activeBatchCount.ToString() + " / " + batchCount);
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

    public static void AddPoint(AdvanceTrailNode node)
    {
        Instance.NewPointInternal(node);
    }
}

public class AdvanceTrailPoint
{
    public Vector3 position;
    public float width;
    public Color color;
    public float life;

    //internal
    public AdvanceTrailNode node;
    public AdvanceTrailRenderer batch;
    public AdvanceTrailPoint lastPoint;
    public int index;


    public AdvanceTrailPoint(AdvanceTrailNode node)
    {
        this.node = node;
    }

    public void UpdatePoint()
    {
        if (batch != null)
            batch.UpdatePoint(this);
    }
}