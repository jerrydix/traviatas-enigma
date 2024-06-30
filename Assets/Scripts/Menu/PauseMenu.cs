using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    
    public PlayerInput inputActions;
    
    void Awake()
    {
       pauseMenu.SetActive(false);
       inputActions = new PlayerInput();
       inputActions.UI.Enable();
    }

    private void Update()
    {
        if (inputActions.UI.Escape.triggered)
        {
          
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
