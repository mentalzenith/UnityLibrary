using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetOffset;
    public float turnSpeed = 10;
    public float minSpeed = 1;
    public float maxSpeed = 3;

    public float arrivalDistance = 1;
    AdvanceTrailNode node;
    TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
    }

    public  void SetNode(AdvanceTrailNode node)
    {
        this.node = node;
    }

    void Update()
    {
        var targetPosition = target.transform.position + targetOffset;
        var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        var distanceSqr = Vector2.SqrMagnitude(targetPosition - transform.position);
        transform.position += transform.forward * Mathf.Lerp(minSpeed, maxSpeed, distanceSqr / (arrivalDistance * arrivalDistance));
    }

    public void ChangeTrailSettings(float width, float life, float pointTimeInterval, float pointDistanceInterval, Color color)
    {
        node.width = width;
        node.life = life;
        node.pointTimeInterval = pointTimeInterval;
        node.pointDistanceInterval = pointDistanceInterval;
        node.color = color;
    }

    public void ChangeTrailColor(Color color)
    {
        node.color = color;
    }

    public void ChangeTargetOffset(Vector3 targetOffset)
    {
        this.targetOffset = targetOffset;
    }

    public void UseUnityTrail(bool isOn)
    {
        node.enabled = !isOn;
        if (trailRenderer != null)
            trailRenderer.enabled = isOn;
    }
}
