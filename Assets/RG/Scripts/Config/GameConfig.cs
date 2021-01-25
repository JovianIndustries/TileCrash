using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Match3/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField]
    private int matchCounter = 3;
    [SerializeField]
    private Level[] gameLevels;
    [SerializeField]
    private List<Tile> gameTiles;

    public int MatchCounter => matchCounter;
    public Level [] GameLevels => gameLevels;
    public List<Tile> GameTiles => gameTiles;

    private void OnValidate() {
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
}