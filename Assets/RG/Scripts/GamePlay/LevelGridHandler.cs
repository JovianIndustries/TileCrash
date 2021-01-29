using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGridHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundPrefab;
    
    private GridCell[,] currentGameGrid;
    private GameObject background;
    
    public PoolManager PoolManager { get; set; }

    public void SpawnLevel(GameData gameData, LevelData levelData) {
        currentGameGrid = levelData.GeneratedGrid;
        
        if(background == null) {
            background = Instantiate(backgroundPrefab, transform);
        }
        var pos = background.transform.position;
        background.transform.position = new Vector3(pos.x, pos.y, 2f);
        background.transform.localScale = levelData.BackgroundScale;
        
        for(int i = 0; i < currentGameGrid.GetLength(1); i++) {
            SpawnTilesLine(i, gameData, levelData);
        }
    }

    public void CleanLeveL(GridCell [,] gameGrid) {
        if(gameGrid == null) {
            gameGrid = new GridCell[0, 0];
        }
        for(int i = 0; i < gameGrid.GetLength(0); i++) {
            for(int j = 0; j < gameGrid.GetLength(1); j++) {
                var gridCell = gameGrid[i, j];
                if(gridCell.HasTile()) {
                    gridCell.Tile.IsMoving = false;
                    PoolManager.ReturnItem(gridCell.Tile.gameObject);
                    gameGrid[i, j].SetTile(null);
                }
            }
        }
    }

    private void SpawnTilesLine(int gridLineIndex, GameData gameData, LevelData levelData) {
        var gridWidth = currentGameGrid.GetLength(0);
        var spawnedTiles = new Tile[gridWidth];

        for(int i = 0; i < gridWidth; i++) {
            List<string> previousIDs = new List<string>();
            if(i >= gameData.MatchCounter-1) {
                for(int j = 1; j < gameData.MatchCounter; j++) {
                    previousIDs.Add(spawnedTiles[i - j].TypeID);
                }
            }
            var currentCell = currentGameGrid[i, gridLineIndex];
            if (!currentCell.HasTile()) {
                spawnedTiles[i] = SpawnTile(gameData, levelData, currentCell, previousIDs);
                spawnedTiles[i].gameObject.SetActive(true);
            }
            previousIDs.Clear();
        }
    }
    
    private Tile SpawnTile(GameData gameData, LevelData levelData, GridCell currentCell, List<string> previousIDs = null) {
        var tiles = gameData.GameTiles;
        var tileToSpawn = GetRandomTile(tiles, previousIDs);
        var spawnedTileObject = PoolManager.GetPoolItem(tileToSpawn.TypeID, transform, levelData.TileScaleMultiplier);
        spawnedTileObject.transform.localPosition = currentCell.CellCenterPos;
        var spawnedTile = spawnedTileObject.GetComponent<Tile>();
        currentCell.SetTile(spawnedTile);
        return spawnedTile;
    }

    // It will avoid creating sets of more than two same type consecutive pieces
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
