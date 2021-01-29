using System.Collections.Generic;

public static class Utils
{
    /// <summary>
    /// Checks if a list of grid cells contains a cell
    /// </summary>
    /// <param name="gridCellList"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public static bool ContainsCell(this List<Cell> gridCellList, Cell cell) {
        for(int i = 0; i < gridCellList.Count; i++) {
            if(cell.x == gridCellList[i].x && cell.y == gridCellList[i].y) {
                return true;
            }
        }
        return false;
    }
}
