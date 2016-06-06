using UnityEngine;
using System.Collections;

public class AdvanceTrailNode : MonoBehaviour
{
    public int persistentPoints = 10;

    public Mode mode;
    public float pointTimeInterval = 0.1f;
    public float pointDistanceInterval;
    public float width = 1;
    public float life = 1;
    public Color color;

    public AdvanceTrailPoint point{ get; private set; }

    public AdvanceTrailPoint lastPoint{ get; private set; }

    AdvanceTrailManager manager;
    AdvanceTrailPoint[] points;
    int index;

    int next{ get { return index + 1 < persistentPoints ? index + 1 : 0; } }

    void Start()
    {
        points = new AdvanceTrailPoint[persistentPoints];
        for (int i = 0; i < persistentPoints; i++)
            points[i] = new AdvanceTrailPoint(this);
        point = points[0];
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
            UpdateOldPoint();
        else
        {
            timeIntervalLeft = pointTimeInterval;
//            UpdateOldPoint();
            UpdateNewPoint();
        }
    }

    void UpdateDistanceMode()
    {        
        float distanceSqr = Vector3.SqrMagnitude(point.position - transform.position);
        if (distanceSqr > pointDistanceInterval * pointDistanceInterval)
            UpdateNewPoint();
        else
            UpdateOldPoint();
    }

    void UpdateNewPoint()
    {
        index = next;
        point = points[index];
        point.lastPoint = lastPoint;
        lastPoint = point;
        UpdatePoint();
        AdvanceTrailManager.AddPoint(this);
    }

    void UpdateOldPoint()
    {
        UpdatePoint();
        AdvanceTrailManager.UpdatePoint(this);
    }

    void UpdatePoint()
    {       
        point.position = transform.position;
        point.width = width;
        point.life = life;
        point.color = color;
    }

    public enum Mode
    {
        time,
        distance
    }
}