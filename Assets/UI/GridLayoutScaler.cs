using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridLayoutScaler : MonoBehaviour {

	[SerializeField] float aspectRatio;

	GridLayoutGroup gridLayoutGroup;
    RectTransform rect;

    void Start ()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup> ();
        rect = transform.parent.GetComponent<RectTransform> ();
    }

    void OnRectTransformDimensionsChange ()
    {
        if (gridLayoutGroup != null && rect != null) {

			float hSpace = gridLayoutGroup.spacing.x;
			float vSpace = gridLayoutGroup.spacing.y;

			float cellSize = 0;
			RectOffset pad = gridLayoutGroup.padding;
			int cc = gridLayoutGroup.constraintCount;
			bool defineByWidth = false;
        	switch (gridLayoutGroup.constraint) {
        		case GridLayoutGroup.Constraint.FixedRowCount:
					cellSize = (rect.rect.size.y-((cc-1)*vSpace)-(pad.top-pad.bottom))/cc;
        			break;

				case GridLayoutGroup.Constraint.FixedColumnCount:
					cellSize = (rect.rect.size.x-((cc-1)*hSpace)-(pad.left+pad.right))/cc;
					defineByWidth = true;
        			break;
        	}

			float scale = 1;
        	CanvasScaler scaler = GetComponentInParent<CanvasScaler>();
        	if (scaler != null) scale = scaler.scaleFactor;

        	cellSize *= scale;

        	// Adjust to aspect
        	float aspectCellSize = cellSize*aspectRatio;

			if (defineByWidth) 
				gridLayoutGroup.cellSize = new Vector2(cellSize, aspectCellSize);
			else
				gridLayoutGroup.cellSize = new Vector2(aspectCellSize, cellSize);
		}
    }
}
