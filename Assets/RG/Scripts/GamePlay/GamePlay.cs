using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GamePlay : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private LevelGridHandler levelGridHandler;

    private LevelData currentLevel;

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
    }

    private void StartTurn(CellPos gridCellPos) {
        DestroyTile(gridCellPos);
        MoveTile(gridCellPos);
        EvaluateMove(gridCellPos);
    }
    
    private void StartTurn(CellPos startCell,  List<CellPos> gridCells) {
        DestroyTiles(gridCells);
        MoveTiles(gridCells);
        EvaluateMove(startCell);
    }

    private void DestroyTiles(List<CellPos> selectedTile) {
        for(int i = 0; i < selectedTile.Count; i++) {
            DestroyTile(selectedTile[i]);
        }
    }
    
    private void DestroyTile(CellPos selectedTile) {
        Debug.Log($"Destroyed {selectedTile.x}, {selectedTile.y}");
        Destroy(levelGridHandler.GetGridCell(currentLevel, selectedTile).OccupyingTile.gameObject);
    }

    
    private void EvaluateMove(CellPos startCell) {
        List<List<CellPos>> tilesToScore = new List<List<CellPos>>();
        var grid = levelGridHandler.CurrentGameGrid;
        
        var startCellY = startCell.y;
        var gridHeight = grid.GetLength(1);
        var gridWidth = grid.GetLength(0);

        //Debug.Log(gridWidth + " " + gridHeight + " " + statCellY);
            
        for(int j = startCellY; j < gridHeight; j++) {
            List<CellPos> candidates = new List<CellPos>();
            for(int k = 0; k < gridWidth - 1; k++) {
                var currentGrid = grid[k, j];
                var nextGrid = grid[k + 1, j];
                Debug.Log($" current grid is {currentGrid.CellPos.ToString()} / next grid is {nextGrid.CellPos.ToString()}");
                var currentPos = new CellPos() {x = currentGrid.CellPos.x, y = currentGrid.CellPos.y};
                var nextPost = new CellPos() {x = nextGrid.CellPos.x, y = nextGrid.CellPos.y};
                
                if(k == 0) {
                    candidates.Add(currentPos);
                    Debug.Log($"Added current cell as {candidates[candidates.Count-1].ToString()}");
                }
                
                if(currentGrid.OccupyingTile.TypeID == nextGrid.OccupyingTile.TypeID) {
                    for(int l = 0; l < candidates.Count; l++) {
                        if(candidates[l].ToString() == currentPos.ToString()) {
                            break;
                        }
                        candidates.Add(currentPos);
                        Debug.Log($"Added current cell as {candidates[candidates.Count-1].ToString()}");
                    }
                    candidates.Add(nextPost);
                    Debug.Log($"Added next cell as {candidates[candidates.Count-1].ToString()}");
                }
                else {
                    if(candidates.Count > 2) {
                        tilesToScore.Add(new List<CellPos>(candidates));
                    }
                    else {
                        candidates.Clear();
                    }
                }
            }
            candidates.Clear();
        }
        
        Debug.Log(tilesToScore.Count);

        if(tilesToScore.Count > 0) {
            List<CellPos> gridCellsToProcess = new List<CellPos>();
            for(int i = 0; i < tilesToScore.Count; i++) {
                gridCellsToProcess.AddRange(tilesToScore[i]);
            }
            
            DestroyTiles(gridCellsToProcess);
            MoveTiles(gridCellsToProcess);
            EvaluateMove(startCell);
            //StartTurn(startCell, gridCellsToProcess);
        }
    }

    private void MoveTiles(List<CellPos> gridCellIDs) {
        for(int i = 0; i < gridCellIDs.Count; i++) {
            MoveTile(gridCellIDs[i]);
        }
    }
    
    private void MoveTile(CellPos gridCellIDs) {
        var grid = levelGridHandler.CurrentGameGrid;
        var gridHeight = grid.GetLength(1);
        var x = gridCellIDs.x;
        var y = gridCellIDs.y;

        for(int j = y + 1; j < gridHeight; j++) {
            var moveFrom = grid[x, j];
            var moveTo = grid[x, j - 1];
            moveFrom.OccupyingTile.transform.position = moveTo.CellCenterPos;
            levelGridHandler.UpdateCell(moveTo, moveFrom.OccupyingTile);
        }
        levelGridHandler.SpawnTile(gameData, currentLevel, grid[x, gridHeight-1]);
    }

    private void OnTileMouseDown(CellPos cellPos) {
        Debug.Log($"Clicked on {cellPos.x}, {cellPos.y}");
        StartTurn(cellPos);
    }
}