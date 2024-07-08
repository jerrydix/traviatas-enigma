using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform camPos;
    private MainMenuManager mainMenuManager;

    // Update is called once per frame

    private void Start()
    {
        if (GameObject.Find("MainMenuUI") != null)
        {
            mainMenuManager = GameObject.Find("MainMenuUI").GetComponent<MainMenuManager>();
        }
    }

    void Update()
    {
        if (mainMenuManager != null && mainMenuManager.isAllowedToMoveCamera)
        {
            transform.position = camPos.position;
        } 
        else if (mainMenuManager == null)
        {
            transform.position = camPos.position;
        }
    }
}