using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private int minAmountEach;
    
    private List<Tile> tilePrefabs;
    private readonly List<GameObject> pool  = new List<GameObject>();
    private readonly List<string> tileTypeId = new List<string>();

    public GameData GameData { private get; set; }
    
    public void Init() {
        if(GameData == null) {
            return;
        }
        tilePrefabs = GameData.GameTiles;
        InitializePool();
    }
    
    public GameObject GetPoolItem(string typeID, Transform parent, float scale, bool asActive = true) {
        var obj = GetPoolTile(typeID);
        obj.transform.SetParent(parent);
        obj.transform.localScale *= scale;
        obj.SetActive(asActive);
        return obj;
    }
    
    /// <summary>
    /// Returns the tile to the pool
    /// </summary>
    /// <param name="tileObj"></param>
    public void ReturnItem(GameObject tileObj) {
        tileObj.SetActive(false);
        tileObj.transform.SetParent(poolParent);
        tileObj.transform.localPosition = Vector3.zero;
        tileObj.transform.localScale = GetScale(tileObj.GetComponent<Tile>());

        var tile = tileObj.GetComponent<Tile>();
        if(tile != null) {
            tile.GridCell = new Cell(0,0);
            tile.IsMoving = false;
        }
    }
    
    private void InitializePool() {
        for(int i = 0; i < tilePrefabs.Count; i++) {
            for(int j = 0; j < minAmountEach; j++) {
                AddObjectToPool(tilePrefabs[i].gameObject);
            }
        }
    }

    private GameObject GetPoolTile(string typeID) {
        GameObject tileObject = null;
        for(int i = 0; i < tileTypeId.Count; i++) {
            if(typeID == tileTypeId[i]) {
                if(!pool[i].activeSelf) {
                    return pool[i];
                }
                tileObject = pool[i];
            }
        }
        return AddObjectToPool(tileObject);
    }
    

    private GameObject AddObjectToPool(GameObject tileObject) {
        var obj = Instantiate(tileObject, poolParent);
        obj.SetActive(false);
        var tile = obj.GetComponent<Tile>();
        if (tile!=null) {
            tile.transform.localScale = GetScale(tile);
            pool.Add(obj);
            tileTypeId.Add(tile.TypeID);
        }
        return obj;
    }

    private Vector3 GetScale(Tile tile) {
        for(int i = 0; i < GameData.GameTiles.Count; i++) {
            if(tile.TypeID == GameData.GameTiles[i].TypeID) {
                return GameData.GameTiles[i].transform.localScale;
            }
        }
        return Vector3.one;
    }
}
