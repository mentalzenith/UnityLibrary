using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
    public GameObject target;
    public float turnSpeed = 10;
    public float minSpeed = 1;
    public float maxSpeed = 3;

    public float arrivalDistance = 1;
    AdvanceTrailNode node;

    public  void SetNode(AdvanceTrailNode node)
    {
        this.node = node;
    }

    void Update()
    {
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        var distanceSqr = Vector2.SqrMagnitude(target.transform.position - transform.position);
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
}
