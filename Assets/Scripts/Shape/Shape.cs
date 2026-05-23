using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shape : MonoBehaviour
{
    public GameObject squareShapeImage;
    public ShapeData CurrentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();

    void Start()
    {
        RequestNewShape(CurrentShapeData);
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        var totalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count < totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach (var square in _currentShape)
        {
            square.gameObject.transform.localPosition = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = new Vector2(
                        GetXPositionForShapeSquare(shapeData, column, moveDistance), 
                        GetYPositionForShapeSquare(shapeData, row, moveDistance)
                    );
                    
                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        return ((shapeData.rows - 1) / 2f - row) * moveDistance.y;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        return (column - (shapeData.columns - 1) / 2f) * moveDistance.x;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;

        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active) number++;
            }
        }

        return number;
    }
}