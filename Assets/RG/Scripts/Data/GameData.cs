using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Match3/Game Config")]
public class GameData : ScriptableObject
{
    [Header("Main Settings")]
    [Tooltip("Amount of pieces to create a match")]
    [SerializeField]
    private int matchCounter = 3;
    [SerializeField]
    private LevelData[] gameLevels;
    [SerializeField]
    private List<Tile> gameTiles;

    [Header("Other Settings")]
    [SerializeField]
    private float tileSpeed = 40f;
    [SerializeField]
    private float tileClickDestroyDelay;
    
    public int MatchCounter => matchCounter;
    public LevelData [] GameLevels => gameLevels;
    public List<Tile> GameTiles => gameTiles;
    public float TileSpeed => tileSpeed;
    public float TileClickDestroyDelay => tileClickDestroyDelay;

#if UNITY_EDITOR
    void OnValidate() {
        for(int i = 0; i < gameTiles.Count; i++) {
            if(gameTiles[i] == null) {
                continue;
            }
            var tiles = gameTiles.ToList();
            tiles.Remove(gameTiles[i]);
            if (tiles.Count > 0 ) {
                for(int j = 0; j < tiles.Count; j++) {
                    if(tiles[j]!=null && tiles[j].TypeID == gameTiles[i].TypeID) {
                        Debug.LogError($"Tile is a duplicate!");
                        gameTiles[i] = null;
                    }
                }
            }
        }
    }
#endif
}
