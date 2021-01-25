using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGridHandler : MonoBehaviour
{
    private GridCell[,] currentGameGrid;
    public GridCell[,] CurrentGameGrid => currentGameGrid;
    
    public void SpawnLevel(GameData gameData, LevelData levelData) {
        currentGameGrid = levelData.GeneratedGrid;
        for(int i = 0; i < currentGameGrid.GetLength(1); i++) {
            SpawnTilesLine(i, gameData, levelData);
        }
    }
    
    public Tile SpawnTile(GameData gameData, LevelData levelData, GridCell currentCell, List<string> previousIDs = null) {
        var tiles = gameData.GameTiles;
        var tileToSpawn = GetRandomTile(tiles, previousIDs);
        var spawnedTileObject = Instantiate(tileToSpawn.gameObject, currentCell.CellCenterPos, Quaternion.identity, transform);
        var scale = spawnedTileObject.transform.localScale;
        var spawnedTile = spawnedTileObject.GetComponent<Tile>();
        spawnedTileObject.transform.localScale = scale * levelData.TileScaleMultiplier;
        UpdateCell(currentCell, spawnedTile);
        return spawnedTile;
    }

    public void UpdateCell(GridCell gridCell, Tile tile) {
        gridCell.OccupyingTile = tile;
        if (tile != null) {
            tile.GridCellPos = gridCell.CellPos;
        }
    }

    public GridCell GetGridCell(LevelData levelData, CellPos cellPosID) {
        return levelData.GeneratedGrid[cellPosID.x, cellPosID.y];
    }
    
    private void SpawnTilesLine(int gridLineIndex, GameData gameData, LevelData levelData) {
        var gridWidth = currentGameGrid.GetLength(0);
        Tile[] spawnedTiles = new Tile[gridWidth];

        for(int i = 0; i < gridWidth; i++) {
            List<string> previousIDs = new List<string>();
            if(i >= gameData.MatchCounter-1) {
                for(int j = 1; j < gameData.MatchCounter; j++) {
                    previousIDs.Add(spawnedTiles[i - j].TypeID);
                }
            }
            var currentCell = currentGameGrid[i, gridLineIndex];
            if (currentCell.OccupyingTile == null) {
                spawnedTiles[i] = SpawnTile(gameData, levelData, currentCell, previousIDs);
            }
            previousIDs.Clear();
        }
    }

    private Tile GetRandomTile(List<Tile> gameTiles, List<string> previousIDs) {
        var tileIndex = Random.Range(0, gameTiles.Count);
        var tile = gameTiles[tileIndex];
        
        if (previousIDs != null) {
            int counter = 0;
            for(int i = 0; i < previousIDs.Count - 1; i++) {
                if(previousIDs[i] != null) {
                    if(tile.TypeID != previousIDs[i]) {
                        break;
                    }
                    counter++;
                }
            }
            if(counter == previousIDs.Count - 1) {
                tile = GetRandomTile(gameTiles, previousIDs);
            }
        }
        return tile;
    }
}