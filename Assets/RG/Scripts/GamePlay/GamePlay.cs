using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private LevelGridHandler levelGridHandler;

    private LevelData currentLevel;
    private GridCell[,] levelGrid;
    private List<GameObject> goToClean = new List<GameObject>();

    public static readonly UnityEvent<Cell> pointerEvent = new UnityEvent<Cell>();
    public static readonly UnityEvent<bool> gameIsStarted = new UnityEvent<bool>();

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

        pointerEvent.AddListener(OnClickedTile);
    }

    public void Play() {
        currentLevel = gameData.GameLevels[2];
        PlayInternal(currentLevel);
    }

    private  void PlayInternal(LevelData selectedLevel) {
        levelGridHandler.CleanLeveL();
        levelGridHandler.SpawnLevel(gameData, selectedLevel);
        levelGrid = levelGridHandler.CurrentGameGrid;
        gameIsStarted.Invoke(true);
    }

    private void StartTurn(Cell cell) {
        RemoveTile(cell);
        MoveTileRow(cell);
        EvaluateMove(cell);
    }

    private void RemoveTile(Cell cell) {
        var gridCell = levelGridHandler.GetGridCell(currentLevel, cell);
        Debug.Log($"Destroyed {cell.ToString()}");
        var go = gridCell.OccupyingTile.gameObject;
        gridCell.SetTile(null);
        go.SetActive(false);
        goToClean.Add(go);
    }

    private void RemoveTiles(List<Cell> cell) {
        for(int i = 0; i < cell.Count; i++) {
            RemoveTile(cell[i]);
            //DebugMarkToRemove(cell[i]);
        }
    }

    private void MoveTileRow(Cell selectedTile) {
        var gridHeight = levelGrid.GetLength(1);
        int i = 0;
        while(selectedTile.y + i < gridHeight - 1) {
            var moveTo = levelGrid[selectedTile.x, selectedTile.y + i];
            var moveFrom = levelGrid[selectedTile.x, selectedTile.y + 1 + i];

            if(moveTo.OccupyingTile == null) {
                if(moveFrom.OccupyingTile != null) {
                    moveFrom.OccupyingTile.transform.position = moveTo.CellCenterPos;
                    moveTo.SetTile(moveFrom.OccupyingTile);
                    moveFrom.SetTile(null);
                }
            }
            i++;
        }
    }

    private void MoveTiles(List<Cell> gridCells) {
        for(int i = 0; i < gridCells.Count; i++) {
            MoveTileRow(gridCells[i]);
        }
    }

    private void EvaluateMove(Cell startCell) {
        List<List<Cell>> tilesToScore = new List<List<Cell>>();
        List<Cell> collector = new List<Cell>();
        var startCellY = startCell.y;
        var gridHeight = levelGrid.GetLength(1);
        var gridWidth = levelGrid.GetLength(0);

        for(int j = startCellY; j < gridHeight; j++) {
            collector.Clear();
            for(int i = 0; i < gridWidth - 1; i++) {
                var currGridCell = levelGrid[i, j];
                var adiacGridCell = levelGrid[i + 1, j];
                
                if(!adiacGridCell.HasTile() || !currGridCell.HasTile()) {
                    continue;
                }
                
                if(currGridCell.OccupyingTile.Equals(adiacGridCell.OccupyingTile)) {
                    if(!collector.ContainsCell(currGridCell.Cell)) {
                        collector.Add(currGridCell.Cell);
                    }
                    collector.Add(adiacGridCell.Cell);
                }
                else if(collector.Count < 3) {
                    collector.Clear();
                }
            }

            if(collector.Count > 2) {
                tilesToScore.Add(new List<Cell>(collector));
            }
        }

        Debug.Log(tilesToScore.Count);

        if(tilesToScore.Count > 0) {
            for(int i = 0; i < tilesToScore.Count; i++) {
                RemoveTiles(tilesToScore[i]);
                MoveTiles(tilesToScore[i]);
            }

            tilesToScore.Clear();
        }
        
        Clean();
    }

    private void DebugMarkToRemove(Cell cell) {
        var gridCell = levelGridHandler.GetGridCell(currentLevel, cell);
        Debug.Log($"cell {cell.ToString()} tile id is {gridCell.OccupyingTile.TypeID}");
        var rend = gridCell.OccupyingTile.GetComponent<MeshRenderer>();
        rend.material.color = Color.cyan;
    }

    private void Clean() {
        for(int i = 0; i < goToClean.Count; i++) {
            Destroy(goToClean[i]);
        }
    }

    private void OnClickedTile(Cell cell) {
        Debug.Log($"Clicked on {cell.x}, {cell.y}");
        StartTurn(cell);
    }

    private void OnDestroy() {
        pointerEvent.RemoveListener(OnClickedTile);
    }
}