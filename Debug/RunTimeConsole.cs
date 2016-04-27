using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class RunTimeConsole : MonoBehaviour
{
	public Text logText;
	
	public void Run ()
	{
		Debug.Log ("Log something");
	}
	
	public void Run2 ()
	{
		
	}

	void Start ()
	{
		Application.RegisterLogCallback (HandleLog);
	}
	
	void HandleLog (string logString, string stackTrace, LogType type)
	{
		logText.text = logString + " \n" + stackTrace;
	}
}
