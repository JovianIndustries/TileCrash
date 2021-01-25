using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    private void Awake() {
        ui?.SetActive(true);
    }
}