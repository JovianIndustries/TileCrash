using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField, ReadOnly]
    private string typeID;

    private Vector3 positionToMove;
    private float speed;
    private bool isMoving;

    public string TypeID => typeID;
    public Cell GridCell { get; set; }

    public bool IsMoving
    {
        set => isMoving = value;
    }

    private void Update() {
        if(!isMoving) {
            return;
        }

        if(Vector3.Distance(transform.position, positionToMove) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, positionToMove, speed * Time.deltaTime);
            return;
        }
        transform.position = positionToMove;
        isMoving = false;
    }

    public void StartMovement(Vector3 position, float tileSpeed) {
        positionToMove = position;
        speed = tileSpeed;
        isMoving = true;
    }
    
    public bool Equals(Tile other) {
        return typeID == other.TypeID;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        Events.TileClickEvent.Invoke(GridCell);
    }
    
#if UNITY_EDITOR
    private void OnValidate() {
        typeID ??= Guid.NewGuid().ToString();
    }
#endif
}
