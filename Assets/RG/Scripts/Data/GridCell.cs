using UnityEngine;

public class GridCell
{
    public CellPos CellPos { get; }
    public Vector3 CellCenterPos { get; }
    public Tile OccupyingTile { get; set; }
    
    public GridCell(CellPos cellPos, Vector2 cellCenterPosition) {
        this.CellPos = cellPos;
        CellCenterPos = cellCenterPosition;
    }
}

public struct CellPos
{
    public int x;
    public int y;

    public override string ToString() {
        return $"[{x}, {y}]";
    }
}