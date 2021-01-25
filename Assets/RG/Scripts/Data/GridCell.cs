using UnityEngine;

public class GridCell 
{
    public Vector2 cellID;
    public Vector3 CellCenterPos { get; }
    public Tile OccupyingTile { get; set; }

    public GridCell(Vector2 cellID, Vector2 cellCenterPosition) {
        this.cellID = cellID;
        CellCenterPos = cellCenterPosition;
    }
}
