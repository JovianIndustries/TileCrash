using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class GamePlay : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private LevelGridHandler levelGridHandler;

    private LevelData currentLevel;

    public static readonly UnityEvent<Tile> pointerEvent = new UnityEvent<Tile>();

    public LevelData CurrentLevelData { get; set; }
    private void Awake() {
        
        if(gameData == null) {
            Debug.LogError("Missing the reference for game config");
            return;
        }

        if(CurrentLevelData == null && gameData.GameLevels.Length > 0) {
            CurrentLevelData = gameData.GameLevels[0];
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

    private void ExecuteTurn(Tile selectedTile) {
        Debug.Log(selectedTile.GridCellID);
        var currentCell = levelGridHandler.GetGridCell(CurrentLevelData, selectedTile.GridCellID);
        DestroyTile(selectedTile);
        MoveTiles(currentCell);
        EvaluateMove(currentCell);
    }

    private void DestroyTile(Tile selectedTile) {
        Destroy(selectedTile.gameObject);
    }
    private void EvaluateMove(GridCell startCell) {
        
    }

    private void MoveTiles(GridCell gridCell) {
        var grid = levelGridHandler.CurrentGameGrid;
        var gridHeight = grid.GetLength(1);
        var x = (int) gridCell.cellID.x;
        var y = (int) gridCell.cellID.y;
        
        for(int i = y + 1; i < gridHeight; i++) {
            var moveFrom = grid[x, i];
            var moveTo = grid[x, i - 1];
            moveFrom.OccupyingTile.transform.position = moveTo.CellCenterPos;
            levelGridHandler.UpdateCell(moveTo, moveFrom.OccupyingTile);
        }

        levelGridHandler.SpawnTile(gameData, currentLevel, grid[x, gridHeight-1]);
    }

    private void OnTileMouseDown(Tile clickedTile) {
        ExecuteTurn(clickedTile);
    }
}