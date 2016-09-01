using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointDrawer : SingletonMonoManager<PointDrawer>
{
    List<PointBatch> pointBatches;
    int currentBatchIndex;

    void Awake()
    {
        pointBatches = new List<PointBatch>();
        CreateBatch();
        currentBatchIndex = 0;
    }

    void Update()
    {
    }

    public static void DrawScreenPoint(Vector2 position, Color Color, float size = 10)
    {
        
    }

    public static void DrawWorldPoint(Vector3 position, Color color, float size = 1)
    {
        var worldPoint = new PointBatch.PointInfo();
        worldPoint.position = position;
        worldPoint.color = color;
        worldPoint.size = size;
        Instance.GetBatch().AddWorldPoint(worldPoint);
    }

    PointBatch GetBatch()
    {
        if (pointBatches[currentBatchIndex].IsFull)
            return CreateBatch();
        else
            return pointBatches[currentBatchIndex];
    }

    PointBatch CreateBatch()
    {
        var batchObject = new GameObject("PointBatch");
        batchObject.transform.SetParent(transform);

        var newBatch = batchObject.AddComponent<PointBatch>();
        pointBatches.Add(newBatch);
        currentBatchIndex++;
        return newBatch;
    }
}
