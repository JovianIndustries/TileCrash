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

    public CellPos(int x, int y) {
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
        var celPos = (CellPos) obj;
        return celPos.x == x && celPos.y == y;
    }

    public bool Equals(CellPos other) {
        return x == other.x && y == other.y;
    }
}