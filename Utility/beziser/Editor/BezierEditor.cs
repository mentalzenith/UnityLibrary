// Name this script "LookAtPointEditor"
//Creates a position handle for the "lookAtPoint" var in the LookAtPoint class
using UnityEngine;
using UnityEditor;

[@CustomEditor (typeof(BezierPoint))]
class LookAtPointEditor : Editor
{
	void OnSceneGUI ()
	{
		var point = (BezierPoint)target;
		EditorGUI.BeginChangeCheck ();
		Vector3 pos = Handles.PositionHandle (point.handle, Quaternion.identity);
		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject (target, "Move LookAt Point");
			point.handle = pos;
			point.OnHandleChanged ();
		}

		Handles.DrawLine (point.transform.position, point.handle);
		Handles.ConeCap (0,
			point.handle,
			Quaternion.LookRotation (point.handle - point.transform.position),
			HandleUtility.GetHandleSize (point.transform.position)*0.3f);  
	}
}