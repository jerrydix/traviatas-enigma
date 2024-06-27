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
    [HideInInspector] public bool pianoMiniGameCompleted;
    [HideInInspector] public bool drumMiniGameCompleted;
    
    [SerializeField] private List<Clock> clocks;
    [HideInInspector] public bool clockMiniGameCompleted; 
    
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
    
    public void CheckKeys()
    {
        
    }
        
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
