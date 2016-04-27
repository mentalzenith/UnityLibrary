using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Test : MonoBehaviour
{
	public LegoManager go;
	public LXFMLData data;

	public void Run ()
	{
		go.Clear ();
	}

	public void Run2 ()
	{
		
	}


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		data = (LXFMLData)FindObjectOfType (typeof(LXFMLData));
	}
}
