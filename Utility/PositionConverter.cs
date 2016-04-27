using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionConverter
{
	public static float canvasScaleFactor;

	public static Vector2 ScreenPointToUI(Vector2 screenPoint)
	{
		return ScreenPointToUI (screenPoint, canvasScaleFactor);
	}

	public static Vector2 ScreenPointToUI (Vector2 screenPoint, float canvasScaleFactor)
	{
		return new Vector2 (screenPoint.x / canvasScaleFactor, (screenPoint.y - Screen.height) / canvasScaleFactor);
	}

	public static Vector2 UIToScreenPoint(Vector2 UIPosition)
	{
		return UIToScreenPoint (UIPosition, canvasScaleFactor);
	}

	public static Vector2 UIToScreenPoint (Vector2 UIPosition, float canvasScaleFactor)
	{
		return new Vector2 (UIPosition.x * canvasScaleFactor, UIPosition.y * canvasScaleFactor + Screen.height);
	}

	public static Vector2 GetScaledScreenPosition (GameObject UIObject, GameObject worldObject)
	{
		return GetScaledScreenPosition (UIObject, worldObject.transform.position);
	}
	
	public static Vector2 GetScaledScreenPosition (GameObject UIObject, Vector3 worldPosition)
	{
		var canvas = UIObject.GetComponentInParent<Canvas> ();
		var scaleFactor = canvas.scaleFactor;
		
		var screenPoint = Camera.main.WorldToScreenPoint (worldPosition);
		var rectTransform = (RectTransform)UIObject.transform;
		
		//scale
		screenPoint = new Vector3 (screenPoint.x / scaleFactor, (screenPoint.y - Screen.height) / scaleFactor, 0);
		
		//adjust to sprite size
		screenPoint = new Vector3 (screenPoint.x - rectTransform.rect.width / 2, screenPoint.y + rectTransform.rect.height / 2, 0);
		return new Vector2 (screenPoint.x, screenPoint.y);
	}
}
