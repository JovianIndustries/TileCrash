using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGridHandler : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;

    private GridCell[,] currentGameGrid;
    public LevelData CurrentLevelData { get; set; }

    void Start() {

        if(gameData == null) {
            Debug.LogError("Missing the reference for game config");
            return;
        }

        if(CurrentLevelData == null && gameData.GameLevels.Length > 0) {
            CurrentLevelData = gameData.GameLevels[0];
        }
        else {
            Debug.LogError("There are not levels configured in the game config", gameData);
        }
    }

    public void SpawnLevel() {
        currentGameGrid = CurrentLevelData.GeneratedGrid;
        for(int i = 0; i < currentGameGrid.GetLength(1); i++) {
            SpawnTilesLine(i, gameData.MatchCounter);
        }
    }

    public void SpawnTilesLine(int lineID, int matchCounter) {
        var gridWidth = currentGameGrid.GetLength(0);
        Tile[] spawnedTiles = new Tile[gridWidth];
        var tiles = gameData.GameTiles;
        List<string> previousIDs = new List<string>();

        for(int i = 0; i < gridWidth; i++) {
            if(i >= matchCounter-1) {
                for(int j = 1; j < matchCounter; j++) {
                    previousIDs.Add(spawnedTiles[i - j].TypeID);
                }
            }

            var tileToSpawn = GetRandomTile(tiles, previousIDs);
            spawnedTiles[i] = tileToSpawn;
            var spawnedTileObject = Instantiate(tileToSpawn.gameObject, currentGameGrid[i, lineID].CellCenterPos, Quaternion.identity, transform);
            var scale = spawnedTileObject.transform.localScale;
            spawnedTileObject.transform.localScale = scale * CurrentLevelData.TileScaleMultiplier;
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