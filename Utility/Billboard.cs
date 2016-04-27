using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	public Camera billboardCamera;
	public bool allowPitch;

	void Awake ()
	{
		billboardCamera = Camera.main;
	}

	void Update ()
	{
		transform.LookAt (transform.position + billboardCamera.transform.rotation * Vector3.forward,
			billboardCamera.transform.rotation * Vector3.up);

		var rotation = transform.localRotation.eulerAngles;

		if (!allowPitch)
			transform.localRotation = Quaternion.Euler (0, rotation.y, rotation.z);
	}
}
