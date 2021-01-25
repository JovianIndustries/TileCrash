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
    public Vector2 GridCellID { get; set; }
    
    private void OnValidate() {
        typeID ??= Guid.NewGuid().ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        GamePlay.pointerEvent.Invoke(this);
    }
}