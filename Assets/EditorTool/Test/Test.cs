﻿using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public float cycleTime;
    public float radius = 1;
    public Vector2 offset;

    public void Run()
    {
    }

    void Update()
    {
        float radian = Mathf.Lerp(0, 2 * Mathf.PI, Time.time % 1);
        transform.position = new Vector3(Mathf.Sin(radian) * radius + offset.x, Mathf.Cos(radian) * radius + offset.y, 0);
    }
}
