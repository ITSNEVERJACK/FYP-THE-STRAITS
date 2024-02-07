using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup { 
    public enum Fittype
    {
        Uniform,
        width,
        Height,
        FixedRows,
        FixedColums
    }

    public Fittype fitType;

    public int rows;

    public int Colums;

    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (fitType == Fittype.width || fitType == Fittype.Height || fitType == Fittype.Uniform)
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            Colums = Mathf.CeilToInt(sqrRt);
        }


        if (fitType == Fittype.width || fitType==Fittype.FixedColums)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)Colums);
        }
        if (fitType == Fittype.Height || fitType==Fittype.FixedRows )
        {
            Colums = Mathf.CeilToInt(transform.childCount / (float)rows);
        }


        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / (float)Colums - ((spacing.x / (float)Colums) * 2) -(padding.left/(float)Colums) -(padding.right/(float)Colums); 
        float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth:cellSize.x;
        cellSize.y = fitY ? cellHeight:cellSize.y;

        int columCount = 0;
        int rowCount = 0;

        for(int i =0; i< rectChildren.Count; i++)
        {
            rowCount = i / Colums;
            columCount = i % Colums;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columCount) + (spacing.x*columCount) + padding.left ;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) +padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }


    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
       
    }

    public override void SetLayoutVertical()
    {
        
    }
}
