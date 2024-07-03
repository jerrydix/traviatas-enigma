using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //todo open smth when this is true
    [SerializeField] private List<Clock> clocks;
    [HideInInspector] public bool clockMiniGameCompleted;
    [SerializeField] private List<Mannequin> mannequins;
    [HideInInspector] public bool mannequinMiniGameCompleted;
    [HideInInspector] public string currentMicrophone;
    [HideInInspector] public bool singingMiniGameCompleted;

    private void Start()
    {
        currentMicrophone = null;
    }

    public void CheckClocks()
    {
        foreach (var clock in clocks)
        {
            if (!clock.clockIsFinished)
            {
                return;
            }
        }

        clockMiniGameCompleted = true;
    }
    
    public void CheckMannequins()
    {
        foreach (var mann in mannequins)
        {
            if (!mann.mannequinCompleted)
            {
                return;
            }
        }
        
        mannequinMiniGameCompleted = true;
    }
}
