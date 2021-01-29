using UnityEngine.Events;

public static class Events
{
    /// <summary>
    /// Called when a tile is clicked
    /// </summary>
    public static readonly UnityEvent<Cell> TileClickEvent = new UnityEvent<Cell>();
    /// <summary>
    /// Called when a game session has started
    /// </summary>
    public static readonly UnityEvent StartSessionEvent = new UnityEvent();
    /// <summary>
    /// Called to quit to Main Menu
    /// </summary>
    public static readonly UnityEvent QuitToMainMenuEvent = new UnityEvent();
}
