using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	Text text;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text> ();
		if (text == null)
			Destroy (this);
	}
	
	// Update is called once per frame
//	void Update ()
//	{
//		if(Time.frameCount%3==0)
//		text.text = (1.0f / Time.deltaTime).ToString ("N0");
//	}

	float deltaTime = 0.0f;

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string fpsString = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		text.text = fpsString;
	}
}
