using UnityEngine;
using System.Collections;

public class AdvanceTrailNode : MonoBehaviour
{
    public int maxPoint = 10;

    public Mode mode;
    public float pointTimeInterval = 0.1f;
    public float pointDistanceInterval;
    public float width;
    public Color color;

    public AdvanceTrailPoint lastPoint{ get; private set; }

    AdvanceTrailManager manager;
    AdvanceTrailPoint[] points;
    int index;

    int next{ get { return index + 1 < maxPoint ? index + 1 : 0; } }

    void Start()
    {
        points = new AdvanceTrailPoint[maxPoint];
        for (int i = 0; i < maxPoint; i++)
            points[i] = new AdvanceTrailPoint(this);
        lastPoint = points[0];
        AdvanceTrailManager.Register(this);
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.time:
                UpdateTimeMode();
                break;
            case Mode.distance:
                UpdateDistanceMode();
                break;
        }
    }

    float timeIntervalLeft;

    void UpdateTimeMode()
    {
        //update time left
        timeIntervalLeft -= Time.deltaTime;
        if (timeIntervalLeft > 0)
            return;
        timeIntervalLeft = pointTimeInterval;

        UpdatePoint();
    }

    void UpdateDistanceMode()
    {        
        float distanceSqr = Vector3.SqrMagnitude(lastPoint.position - transform.position);
        if (distanceSqr > pointDistanceInterval * pointDistanceInterval)
            UpdatePoint();
    }

    void UpdatePoint()
    {
        index = next;
        var point = points[index];
        point.position = transform.position;
        point.width = width;
        point.color = color;
        point.lastPoint = lastPoint;
        lastPoint = point;

        AdvanceTrailManager.UpdatePoint(this);
    }

    public enum Mode
    {
        time,
        distance
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
    public int index;

    public AdvanceTrailPoint lastPoint;

    public AdvanceTrailPoint(AdvanceTrailNode node)
    {
        this.node = node;
    }

    public void UpdateNormal(Vector3 nextPoint)
    {
        
    }
}