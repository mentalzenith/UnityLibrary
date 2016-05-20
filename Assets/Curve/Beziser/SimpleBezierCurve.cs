using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleBezierCurve : MonoBehaviour
{

	//public List<BezierPoint> points;

	public BezierPoint start;
	public BezierPoint end;

	void UpdateDebugCurve ()
	{
		Vector3 lastPoint = start.transform.position;
		for (int i = 0; i < 20; i++)
		{
			var point = CalculateBezierPoint ((float)i / 20, 
				            start.point, 
				            start.handle,
				            end.handle,
				            end.point);
			Debug.DrawLine (lastPoint, point);
			lastPoint = point;
		}
	}


	public Vector3 CalculateBezierPoint (float t)
	{
		return CalculateBezierPoint (t, start.point, start.handle, end.handle, end.point);
	}

	Vector3 CalculateBezierPoint (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		var u = 1.0f - t;
		var tt = t * t;
		var uu = u * u;
		var uuu = uu * u;
		var ttt = tt * t;

		var p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u * tt * p2; //third term
		p += ttt * p3; //fourth term

		return p;
	}

	void OnDrawGizmos ()
	{
		Vector3 lastPoint = start.transform.position;
		for (int i = 0; i <= 20; i++)
		{
			var point = CalculateBezierPoint ((float)i / 20);
			Gizmos.DrawLine (lastPoint, point);
			lastPoint = point;
		}
	}
}