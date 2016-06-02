using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public float cycleTime;
    public float radius = 1;

    public void Run()
    {
    }

    void Update()
    {
        float radian = Mathf.Lerp(0, 2 * Mathf.PI, Time.time % 1);
        transform.position = new Vector3(Mathf.Sin(radian) * radius, Mathf.Cos(radian) * radius, 0);
    }
}
