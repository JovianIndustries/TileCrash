using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private string typeID;

    public string TypeID => typeID;
    public GridCell CurrentCell { get; set; }
    
    private void OnValidate() {
        typeID ??= Guid.NewGuid().ToString();
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("clicked on " + CurrentCell.cellID);
    }
}