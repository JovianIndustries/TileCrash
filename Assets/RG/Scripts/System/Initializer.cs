using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private GamePlay gamePlay;
    private LevelGridHandler levelGridHandler;
    private UIHandler uiHandler;
    
    private void Awake() {
        if(gameData == null) {
            Debug.LogError("Missing GameData reference", this);
        }
        gamePlay = FindObjectOfType<GamePlay>();
        levelGridHandler = FindObjectOfType<LevelGridHandler>();
        uiHandler = FindObjectOfType<UIHandler>();
        var poolManager = GetComponent<PoolManager>();

        if(poolManager != null) {
            Debug.Log("Pool initialized");
            poolManager.GameData = gameData;
            poolManager.Init();
        }
        else {
            Debug.LogError("Failed to initialize pool");
        }
        
        if(gamePlay != null) {
            gamePlay.PoolManager = poolManager;
            gamePlay.GameData = gameData;
            Debug.Log("PoolManager, GameData Injected in the GamePlay");
            gamePlay.Init();
        }
        else {
            Debug.LogError("Failed to initialize GamePlay");
        }
        
        if(levelGridHandler != null) {
            levelGridHandler.PoolManager = poolManager;
            Debug.Log("PoolManager Injected in the LevelGridHandler");
        }        
        else {
            Debug.LogError("Failed to initialize LevelHandler");
        }
        
        if(uiHandler != null) {
            uiHandler.GameData = gameData;
            uiHandler.ToggleMainMenu(true);
            Debug.Log("GameData Injected in the UIHandler");
        }        
        else {
            Debug.LogError("Failed to initialize UIHandler");
        }
    }
}
