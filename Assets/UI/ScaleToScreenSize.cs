using UnityEngine;
using System.Collections;

public class ScaleToScreenSize : MonoBehaviour
{
	void Start()
	{	
		var scaleFactor = GetComponentInParent<Canvas>().scaleFactor;
		var rectTransform = GetComponent<RectTransform>();
		var scaledScreenHeight = (Screen.height) / scaleFactor;
		rectTransform.sizeDelta = new Vector2(0, scaledScreenHeight);
		rectTransform.anchoredPosition = new Vector2(0, -scaledScreenHeight / 2);
	}
}
