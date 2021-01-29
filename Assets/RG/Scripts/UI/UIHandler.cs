using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject menuParent;
    [SerializeField]
    private GameObject levelSelectionMenu;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private SessionData sessionData;
    
    private bool isActive;
    private bool isGameSessionActive;
    public GameData GameData { private get; set; }

    void Start() {
        Events.QuitToMainMenuEvent.AddListener(EndSession);
        menuParent.SetActive(true);
        levelSelectionMenu.SetActive(false);
        SetLevelSelectionMenu();
    }

    void Update() {
        if(Keyboard.current.escapeKey.wasPressedThisFrame) {
            ToggleMainMenu(!isActive);
        }
    }
    
    public void ToggleMainMenu(bool value) {
        if(isGameSessionActive) {
            levelSelectionMenu.SetActive(false);
            menuParent.SetActive(value);
            isActive = value;   
        }
    }
    
    public void Quit() {
        Application.Quit();
    }
    
    private void SetLevelSelectionMenu() {
        for(int i = 0; i < GameData.GameLevels.Length; i++) {
            var button = Instantiate(buttonPrefab, levelSelectionMenu.transform).GetComponent<Button>();
            var textField = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var index = i;
            textField.text = $"Test Case {i+1}";
            button.onClick.AddListener(() => SetLevelAndPlay(index));
        }
    }

    private void SetLevelAndPlay(int levelIndex) {
        Events.StartSessionEvent.Invoke();
        sessionData.currenLevelIndex = levelIndex;
        Events.StartSessionEvent.Invoke();
        isGameSessionActive = true;
        ToggleMainMenu(false);
    }

    private void EndSession() {
        ToggleMainMenu(true);
        isGameSessionActive = false;
    }

    private void OnDestroy() {
        Events.QuitToMainMenuEvent.RemoveListener(EndSession);
    }
}
