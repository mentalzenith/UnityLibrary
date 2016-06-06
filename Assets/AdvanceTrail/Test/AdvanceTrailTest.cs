using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailTest : MonoBehaviour
{
    [Header("Camera")]
    public TrackingMode trackingMode;
    public float cameraDistance;

    [Header("Trail")]
    public float life = 1;
    public float width = 1;
    public float pointTimeInterval = 0.1f;
    public float pointDistanceInterval;
    public Color color = Color.white;
    public bool randomColor;

    [Header("Boids")]
    [Range(0, 100)]
    public int boidNumber = 10;

    public float minSpeed = 0.5f;
    public float minSpeedVariation = 0.2f;
    public float maxSpeed = 2;
    public float maxSpeedVariation = 0.2f;
    public float turnSpeed = 10;
    public float turnSpeedVariation = 0.2f;

    [Header("Target")]
    public GameObject target;
    public float cycleTime = 10;
    public float radius = 100;
    public Vector2 offset;

    bool isDirty;

    List<Boid> boids;

    void Awake()
    {
        boids = new List<Boid>();
        AdjustBoids();
        UpdateTrailSettings();
    }

    void OnValidate()
    {
        isDirty = true;
    }

    void Update()
    {
        Track();
        float radian = Mathf.Lerp(0, 2 * Mathf.PI, Time.time % cycleTime / cycleTime);
        target.transform.position = new Vector3(Mathf.Sin(radian) * radius + offset.x, Mathf.Cos(radian) * radius + offset.y, 0);

        if (randomColor)
        {
            color = new Color(Mathf.Sin(radian), Mathf.Cos(radian), Mathf.Tan(radian));
            UpdateTrailColor();
        }

        if (!isDirty)
            return;
        
        AdjustBoids();
        UpdateTrailSettings();
        isDirty = false;
    }

    void Track()
    {
        var boidTransform = boids[0].transform;
        switch (trackingMode)
        {
            case TrackingMode.target:
                Camera.main.transform.LookAt(target.transform);
                break;
            case TrackingMode.boidFirstPerson:
                Camera.main.transform.LookAt(boidTransform.position);
                Camera.main.transform.position = boidTransform.position - boidTransform.forward * cameraDistance;
                break;
            case TrackingMode.boidThirdPerson:
                Camera.main.transform.LookAt(boidTransform.position);
                Camera.main.transform.position = boidTransform.position - new Vector3(cameraDistance, 0, 0);
                break;
        }
    }

    void AdjustBoids()
    {
        boidNumber = Mathf.Clamp(boidNumber, 1, 100);

        int boidCount = boids.Count;
        while (boidCount++ < boidNumber)
            SpawnBoid();
        
        boidCount = boids.Count;
        while (boidCount-- > boidNumber)
        {
            int last = boids.Count - 1;
            Destroy(boids[last].gameObject);
            boids.RemoveAt(last);
        }
    }

    Boid SpawnBoid()
    {
        var boid = new GameObject("Boid").AddComponent<Boid>();
        boid.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        boid.target = target;
        
        boid.SetNode(boid.gameObject.AddComponent<AdvanceTrailNode>());
        boids.Add(boid);
        return boid;
    }

    void UpdateTrailSettings()
    {
        foreach (var boid in boids)
        {
            boid.ChangeTrailSettings(width, life, pointTimeInterval, pointDistanceInterval, color);
            boid.minSpeed = minSpeed + Random.Range(minSpeed * -minSpeedVariation, minSpeed * minSpeedVariation);
            boid.maxSpeed = maxSpeed + Random.Range(maxSpeed * -maxSpeedVariation, maxSpeed * maxSpeedVariation);
            boid.turnSpeed = turnSpeed + Random.Range(turnSpeed * -turnSpeedVariation, turnSpeed * turnSpeedVariation);
        }
    }

    void UpdateTrailColor()
    {
        foreach (var boid in boids)
            boid.ChangeTrailColor(color);
    }

    public enum TrackingMode
    {
        target,
        boidFirstPerson,
        boidThirdPerson
    }
}
