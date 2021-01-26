using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Level00Config", menuName = "Match3/Level Config")]
public class LevelData : ScriptableObject
{
    [Tooltip("A generic identifier for the level")]
    [SerializeField]
    private string levelID = "Optional";
    [SerializeField]
    private Vector3 bottomLeftGridAnchor;
    [SerializeField]
    private Vector3 topRightGridAnchor;
    [Tooltip("Must specify integer values. X = length, Y = height")]
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private float tileScaleMultiplier = 1f;

    private GridCell[,] generatedGrid;
    public GridCell[,] GeneratedGrid {
        get {
            if (generatedGrid == null) {
                GenerateCellGrid();
            }
            return generatedGrid;
        }
    }
    public float TileScaleMultiplier => tileScaleMultiplier;

    private void GenerateCellGrid() {
        var gridLength = Mathf.Abs(topRightGridAnchor.x - bottomLeftGridAnchor.x);
        var gridHeight = Mathf.Abs(topRightGridAnchor.y - bottomLeftGridAnchor.y);
        
        var celEdgeLength = gridLength / gridSize.x;
        var horizCelNo = (int)Mathf.Floor(gridLength / celEdgeLength);
        var vertCelNo = (int)Mathf.Floor(gridHeight / celEdgeLength);
        
        generatedGrid = new GridCell [horizCelNo, vertCelNo];

        for(var i = 0; i < horizCelNo; i++) {
            for(var j = 0; j < vertCelNo; j++) {
                var cell = new Cell{x = i, y = j};
                GeneratedGrid[i, j] = new GridCell(cell, GetCellCenterPosition(cell));
            }
        }

        Vector3 GetCellCenterPosition(Cell cell) {
            var cellAnchor = new Vector2(bottomLeftGridAnchor.x + cell.x * celEdgeLength, bottomLeftGridAnchor.y + cell.y * celEdgeLength);
            var cellCenterPos = new Vector2(cellAnchor.x + celEdgeLength / 2, cellAnchor.y + celEdgeLength / 2);
            // DebugDrawCell(cellAnchor);
            return cellCenterPos;
        }

        void DebugDrawCell(Vector3 cellAnchor) {
            cellAnchor.z = 0f;
            Debug.DrawLine(cellAnchor, new Vector3(cellAnchor.x + celEdgeLength, cellAnchor.y), Color.blue, 60, false);
            Debug.DrawLine(cellAnchor, new Vector3(cellAnchor.x, cellAnchor.y + celEdgeLength), Color.yellow, 60, false);
        }
    }
    
    void OnValidate() {
        gridSize.x = Mathf.RoundToInt(gridSize.x);
        gridSize.y = Mathf.RoundToInt(gridSize.y);
    }

}