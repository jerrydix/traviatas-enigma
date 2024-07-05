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
    
    [SerializeField] private List<Piano> pianos;
    [HideInInspector] int amountPianosCompleted; //0-3
    [SerializeField] private List<Drum> drums;
    [HideInInspector] int amountDrumsCompleted; //0-3
    [SerializeField] private List<MajorMinor> majorMinors;
    [HideInInspector] int amountMajorMinorsCompleted; //0-3
    [SerializeField] private List<Mannequin> mannequins;
    [HideInInspector] public bool mannequinMiniGameCompleted;
    [SerializeField] private List<Clock> clocks;
    [HideInInspector] public bool clockMiniGameCompleted;
    [HideInInspector] public bool motorMinigameCompleted;
    
    [HideInInspector] public bool singingMiniGameCompleted;


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
    
    public void CheckMajorMinors()
    {
        amountMajorMinorsCompleted = 0;
        foreach (var majorMinor in majorMinors)
        {
            if (majorMinor.majorMinorMiniGameCompleted)
            {
                amountMajorMinorsCompleted++;
            }
        }
    }
    
    public void CheckDrums()
    {
        amountDrumsCompleted = 0;
        foreach (var drum in drums)
        {
            if (drum.rhythmMiniGameCompleted)
            {
                amountDrumsCompleted++;
            }
        }
    }
    
    public void CheckPianos()
    {
        amountPianosCompleted = 0;
        foreach (var piano in pianos)
        {
            if (piano.pianoMiniGameCompleted)
            {
                amountPianosCompleted++;
            }
        }
    }
}
