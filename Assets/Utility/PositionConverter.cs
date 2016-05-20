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
}
