using UnityEngine;
using System.Collections;

using UnityEngine;
using System;

[Serializable]
public class BezierPoint : MonoBehaviour
{
	public event Action OnPointChanged;

	public Vector3 point { 
		get { return transform.position; }
		set { transform.position = value; }
	}

	//local offset
	[SerializeField]
	Vector3 _handle = new Vector3 (1, 0, 0);

	public Vector3 handle { 
		get { return transform.position + _handle; } 
		set { _handle = value - transform.localPosition; }
	}

	public void OnHandleChanged ()
	{
		if (OnPointChanged != null)
			OnPointChanged ();
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawSphere (transform.position, 0.5f);
	}
}