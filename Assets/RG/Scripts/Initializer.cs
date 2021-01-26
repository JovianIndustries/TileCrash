using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private UIHandler uiHandler;

    private void Awake() {
        uiHandler.Togglemenu(true);
    }
}