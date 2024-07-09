using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
        
        instance = RuntimeManager.CreateInstance(sounds);
        longInstance = RuntimeManager.CreateInstance(longSounds);
        longInstance.start();
    }
    
    [SerializeField] private EventReference sounds;
    [SerializeField] private EventReference longSounds;

    private EventInstance instance;
    private EventInstance longInstance;

    [HideInInspector] public bool isSFXPlaying;
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

    private bool isInDuck;
    
    public void PlayLong()
    {
        StartCoroutine(PlayLongSound());
    }

    public void PlayShort()
    {
        StartCoroutine(PlaySound());
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
    
    IEnumerator PlaySound()
    {
        isSFXPlaying = true;
        instance.start();
        yield return new WaitForSeconds(10f);
        isSFXPlaying = false;
    }

    public void DisableMusic()
    {
        longInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    IEnumerator PlayLongSound()
    {
        isSFXPlaying = true;
        float fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime * 0.1f;
            longInstance.setParameterByName("Crossfade", fade);
            yield return null;
        }
        yield return new WaitForSeconds(20f);
        while (fade > 0)
        {
            fade -= Time.deltaTime * 0.1f;
            longInstance.setParameterByName("Crossfade", fade);
            yield return null;
        }
        isSFXPlaying = false;
    }

    public void ApplyDucking()
    {
        if (!isInDuck)
        {
            StartCoroutine(Duck());
            isInDuck = true;
        }
    }
    
    public void DisableDucking()
    {
        if (isInDuck)
        {
            StartCoroutine(UnDuck());
            isInDuck = false;
        }
    }
    
    IEnumerator Duck()
    {
        float value = 0;
        Debug.Log(value);
        while (value < 1)
        {
            Debug.Log(value);
            value += Time.deltaTime * 0.5f;
            RuntimeManager.StudioSystem.setParameterByName("GlobalDucking", value);
            yield return null;
        }
        Debug.Log(value);

        RuntimeManager.StudioSystem.setParameterByName("GlobalDucking", 1);
    }

    IEnumerator UnDuck()
    {
        float value = 1;
        while (value > 0)
        {
            value -= Time.deltaTime * 0.4f;
            RuntimeManager.StudioSystem.setParameterByName("GlobalDucking", value);
            yield return null;
        }
        RuntimeManager.StudioSystem.setParameterByName("GlobalDucking", 0);
    }

    private void OnDestroy()
    {
        longInstance.stop(STOP_MODE.IMMEDIATE);
    }
}
