using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenu;
    private bool isActive;
    private bool gameIsStarted;

    private void Start() {
        GamePlay.gameIsStarted.AddListener(GameIsRunning);
    }

    private void GameIsRunning(bool value) {
        gameIsStarted = value;
    }

    void Update() {
        if(Keyboard.current.escapeKey.wasPressedThisFrame) {
            if (gameIsStarted) {
                Togglemenu(!isActive);
            }
        }
    }

    public void Togglemenu(bool value) {
        PauseMenu.SetActive(value);
        isActive = value;
    }

    private void OnDestroy() {
        GamePlay.gameIsStarted.RemoveListener(GameIsRunning);
    }
}
