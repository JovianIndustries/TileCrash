using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GamePlay : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private LevelGridHandler levelGridHandler;

    private LevelData currentLevel;
    private GridCell[,] levelGrid;
    private List<GameObject> goToClean = new List<GameObject>();
    
    public static readonly UnityEvent<CellPos> pointerEvent = new UnityEvent<CellPos>();

    private void Awake() {
        
        if(gameData == null) {
            Debug.LogError("Missing the reference for game config");
            return;
        }

        if(currentLevel == null && gameData.GameLevels.Length > 0) {
            currentLevel = gameData.GameLevels[0];
        }
        else {
            Debug.LogError("There are not levels configured in the game config", gameData);
            return;
        }
        
        pointerEvent.AddListener(OnTileMouseDown);
    }

    public void Play() {
        currentLevel = gameData.GameLevels[0];
        PlayInternal(currentLevel);
    }
    
    private  void PlayInternal(LevelData selectedLevel) {
        levelGridHandler.SpawnLevel(gameData, selectedLevel);
        levelGrid = levelGridHandler.CurrentGameGrid;
    }

    private void StartTurn(CellPos gridCellPos) {
        RemoveTile(gridCellPos);
        MoveTile(gridCellPos);
        EvaluateMove(gridCellPos);
    }

    private void RemoveTile(CellPos selectedTile) {
        var gridCell =levelGridHandler.GetGridCell(currentLevel, selectedTile);
        Debug.Log($"Destroyed {selectedTile.ToString()}");
        var go = gridCell.OccupyingTile.gameObject;
        go.SetActive(false);
        goToClean.Add(go);
        levelGridHandler.UpdateCell(gridCell, null);
    }
    
    private void RemoveTiles(List<CellPos> selectedTile) {
        for(int i = 0; i < selectedTile.Count; i++) {
            RemoveTile(selectedTile[i]);
        }
    }
    
    private void MoveTile(CellPos selectedTile) {
        var gridHeight = levelGrid.GetLength(1);
        var x = selectedTile.x;
        var y = selectedTile.y + 1;
        var tileToMove = y;
        var top = levelGrid[x, gridHeight - 1];
        GridCell nullCell = levelGrid[x, y];
        
        while(true) {
            if(y == gridHeight) {
                break;
            }
            if(y == gridHeight - 1) {
                levelGridHandler.SpawnTile(gameData, currentLevel, top);
            }
            var moveFrom = levelGrid[x, y];
            var moveTo = levelGrid[x, selectedTile.y];
            if(moveFrom.OccupyingTile != null) {
                moveFrom.OccupyingTile.transform.position = moveTo.CellCenterPos;
                levelGridHandler.UpdateCell(moveTo, moveFrom.OccupyingTile);
            }
            else {
                nullCell = moveFrom;
            }

            y++;
            
            if(tileToMove == gridHeight - 1 && nullCell != null) {
                MoveTile(nullCell.CellPos);
                break;
            }
        }
        
        // var gridHeight = levelGrid.GetLength(1);
        // var x = selectedTile.x;
        // var y = selectedTile.y + 1;
        //
        // for(int j = y; j < gridHeight; j++) {
        //     var moveFrom = levelGrid[x, j];
        //     var moveTo = levelGrid[x, j - 1];
        //     if(moveFrom.OccupyingTile != null) {
        //         moveFrom.OccupyingTile.transform.position = moveTo.CellCenterPos;
        //         levelGridHandler.UpdateCell(moveTo, moveFrom.OccupyingTile);
        //     }
        //     else {
        //         MoveTile(moveFrom.CellPos);
        //     }
        // }
        // var top = levelGrid[x, gridHeight - 1];
        // if (top.OccupyingTile == null) {
        //     levelGridHandler.SpawnTile(gameData, currentLevel, levelGrid[x, gridHeight - 1]);
        // }
        // else {
        //     Debug.LogError("Tile already present at that position!");
        // }
    }

    
    private void MoveTiles(List<CellPos> gridCellIDs) {
        for(int i = 0; i < gridCellIDs.Count; i++) {
            MoveTile(gridCellIDs[i]);
        }
    }

    private void EvaluateMove(CellPos startCell) {
        List<List<CellPos>> tilesToScore = new List<List<CellPos>>();
        List<CellPos> collector = new List<CellPos>();
        var startCellY = startCell.y;
        var gridHeight = levelGrid.GetLength(1);
        var gridWidth = levelGrid.GetLength(0);
        
        while(true) {
            //Debug.Log(gridWidth + " " + gridHeight + " " + statCellY);

            for(int j = startCellY; j < gridHeight; j++) {
                for(int i = 0; i < gridWidth - 1; i++) {
                    if(levelGrid[i, j].OccupyingTile.Equals(levelGrid[i + 1, j].OccupyingTile)) {
                        if(collector.Count == 0) {
                            collector.AddRange(new List<CellPos>() {new CellPos() {x = i, y = j}, new CellPos() {x = i + 1, y = j}});
                        }
                        else {
                            collector.Add(new CellPos() {x = i + 1, y = j});
                        }
                    }
                }

                if(collector.Count > 2) {
                    tilesToScore.Add(new List<CellPos>(collector));
                }
                
                collector.Clear();
            }

            Debug.Log(tilesToScore.Count);

            if(tilesToScore.Count > 0) {
                for(int i = 0; i < tilesToScore.Count; i++) {
                    RemoveTiles(tilesToScore[i]);
                    MoveTiles(tilesToScore[i]);
                }
                tilesToScore.Clear();
                continue;
            }
            break;
        }

        Clean();
    }

    private void Clean() {
        for(int i = 0; i < goToClean.Count; i++) {
            Destroy(goToClean[i]);
        }
    }

    private void OnTileMouseDown(CellPos cellPos) {
        Debug.Log($"Clicked on {cellPos.x}, {cellPos.y}");
        StartTurn(cellPos);
    }
}