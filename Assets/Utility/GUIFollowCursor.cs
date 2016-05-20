using UnityEngine;
using System.Collections;

public class GUIFollowCursor : MonoBehaviour
{
	void Update ()
	{
		var rectTransform = (RectTransform)transform;
		rectTransform.anchoredPosition = PositionConverter.ScreenPointToUI (Input.mousePosition,GetComponentInParent<Canvas>().scaleFactor);
	}
}
