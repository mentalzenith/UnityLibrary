using UnityEngine;
using UnityEditor;

[@CustomEditor (typeof(SimpleBezierCurve))]
public class SimpleBezierCurveEditor : Editor
{
	void OnSceneGUI ()
	{
//		var curve = (SimpleBezierCurve)target;
//		Vector3 lastPoint = curve.start.transform.position;
//		for (int i = 0; i < 20; i++)
//		{
//			var point = curve.CalculateBezierPoint ((float)i / 20);
//			Debug.DrawLine (lastPoint, point);
//			lastPoint = point;
//		}
	}
}
