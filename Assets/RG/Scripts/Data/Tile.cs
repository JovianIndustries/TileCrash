using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField][ReadOnly]
    private string typeID;

    public string TypeID => typeID;
    public CellPos GridCellPos { get; set; }
    
    private void OnValidate() {
        typeID ??= Guid.NewGuid().ToString();
    }
    
    public bool Equals(Tile other) {
        return typeID == other.TypeID;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        GamePlay.pointerEvent.Invoke(GridCellPos);
    }
}