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
    private bool pianosComplete;
    [SerializeField] private List<Drum> drums;
    [HideInInspector] int amountDrumsCompleted; //0-3
    private bool drumsComplete;
    [SerializeField] private List<MajorMinor> majorMinors;
    [HideInInspector] int amountMajorMinorsCompleted; //0-3
    private bool majorMinorComplete;
    [SerializeField] private List<Mannequin> mannequins;
    [HideInInspector] int amountMannequins;
    [HideInInspector] public bool mannequinMiniGameCompleted;
    [SerializeField] private List<Clock> clocks;
    [HideInInspector] public bool clockMiniGameCompleted;
    [HideInInspector] public bool motorMinigameCompleted;
    
    [HideInInspector] public bool singingMiniGameCompleted;
    [SerializeField] private S_MainDoor mainDoor;


    public void CheckClocks()
    {
        foreach (var clock in clocks)
        {
            if (clock.clockIsFinished)
            {
                return;
            }
        }
        clockMiniGameCompleted = true;
        mainDoor.TurnLamp(3,3);
        CheckGames();
    }
    
    public void CheckMannequins()
    {
        amountMannequins = 0;
        foreach (var mann in mannequins)
        {
            if (mann.mannequinCompleted)
            {
                amountMannequins++;
            }
        }
        
        mainDoor.TurnLamp(4,amountMannequins);
        
        if (amountMannequins == 3)
        {
            mannequinMiniGameCompleted = true;
        }
        CheckGames();
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
        mainDoor.TurnLamp(2,amountMajorMinorsCompleted);

        if (amountMajorMinorsCompleted == 3)
        {
            majorMinorComplete = true;
        }
        CheckGames();
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
        mainDoor.TurnLamp(1,amountDrumsCompleted);

        if (amountDrumsCompleted == 3)
        {
            drumsComplete = true;
        }
        CheckGames();
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
        mainDoor.TurnLamp(0,amountPianosCompleted);

        if (amountPianosCompleted == 3)
        {
            pianosComplete = true;
        }
        CheckGames();
    }

    public void SetMotor()
    {
        motorMinigameCompleted = true;
        mainDoor.TurnLamp(5,3);
        CheckGames();
    }

    private void CheckGames()
    {
        if (motorMinigameCompleted && mannequinMiniGameCompleted && pianosComplete && drumsComplete &&
            majorMinorComplete && clockMiniGameCompleted)
        {
            mainDoor.OpenDoor();
        }
    }
}
