using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGridHandler : MonoBehaviour
{
    [SerializeField]
    private GameConfig gameConfig;

    private GridCell[,] currentGameGrid;
    public Level CurrentLevel { get; set; }

    void Start() {

        if(gameConfig == null) {
            Debug.LogError("Missing the reference for game config");
            return;
        }

        if(CurrentLevel == null && gameConfig.GameLevels.Length > 0) {
            CurrentLevel = gameConfig.GameLevels[0];
        }
        else {
            Debug.LogError("There are not levels configured in the game config", gameConfig);
        }
    }

    public void SpawnLevel() {
        currentGameGrid = CurrentLevel.GeneratedGrid;
        for(int i = 0; i < currentGameGrid.GetLength(1); i++) {
            SpawnTilesLine(i, gameConfig.MatchCounter);
        }
    }

    private void SpawnTilesLine(int lineID, int matchCounter) {
        var gridWidth = currentGameGrid.GetLength(0);
        Tile[] spawnedTiles = new Tile[gridWidth];
        var tiles = gameConfig.GameTiles;
        List<string> previousIDs = new List<string>();

        for(int i = 0; i < gridWidth; i++) {
            if(i >= matchCounter) {
                for(int j = 1; j < matchCounter+1; j++) {
                    previousIDs.Add(spawnedTiles[i - j].TypeID);
                }
            }

            var tileToSpawn = GetRandomTile(tiles, previousIDs);
            spawnedTiles[i] = tileToSpawn;
            
            var spawnedTileObject = Instantiate(tileToSpawn.gameObject, currentGameGrid[i, lineID].CellCenterPos, Quaternion.identity, transform);
            var scale = spawnedTileObject.transform.localScale;
            spawnedTileObject.transform.localScale =
                new Vector3(scale.x * tileToSpawn.ScaleFactor, scale.y * tileToSpawn.ScaleFactor, scale.z * tileToSpawn.ScaleFactor);
            var spawnedTile = spawnedTileObject.GetComponent<Tile>();
            currentGameGrid[i, lineID].OccupyingTile = spawnedTile;
            spawnedTile.CurrentCell = currentGameGrid[i, lineID];
            previousIDs.Clear();
        }
    }

    private Tile GetRandomTile(List<Tile> gameTiles, List<string> previousIDs) {
        var tileIndex = Random.Range(0, gameTiles.Count);
        var tile = gameTiles[tileIndex];
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

        return tile;
    }
}

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