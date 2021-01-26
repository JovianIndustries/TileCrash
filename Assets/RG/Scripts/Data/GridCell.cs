using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    
    private Tile occupyingTile;
    private readonly Cell cell;

    public Cell Cell => cell;
    public Vector3 CellCenterPos { get; }
    public Tile OccupyingTile => occupyingTile;

    public GridCell(Cell cell, Vector2 cellCenterPosition) {
        this.cell = cell;
        CellCenterPos = cellCenterPosition;
    }

    public bool HasTile() {
        return OccupyingTile != null;
    }

    public void SetTile(Tile tile) {
        occupyingTile = tile;
        if (tile != null) {
            tile.GridCell = cell;
        }
    }
}

public struct Cell
{
    public int x;
    public int y;

    public Cell(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    public override string ToString() {
        return $"[{x}, {y}]";
    }
    
    public override bool Equals(object obj) {
        if ((obj == null) || GetType() != obj.GetType())
        {
            return false;
        }
        var cell = (Cell) obj;
        return cell.x == x && cell.y == y;
    }
}