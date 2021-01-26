using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool ContainsCell(this List<Cell> cellList, Cell cell) {
        var result = false;
        for(int i = 0; i < cellList.Count; i++) {
            if(cell.x == cellList[i].x && cell.y == cellList[i].y) {
                result = true;
                break;
            }
        }
        return result;
    }
}
