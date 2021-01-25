using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private string typeID;
    [SerializeField]
    private float scaleFactor = 1f;
    
    public string TypeID => typeID;
    public float ScaleFactor => scaleFactor;
    public GridCell CurrentCell { get; set; }
    
    private void OnValidate() {
        if(typeID == null) { 
            typeID = Guid.NewGuid().ToString();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("clicked on " + CurrentCell.cellID);
    }
}