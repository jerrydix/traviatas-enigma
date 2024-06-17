using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class Drum : MonoBehaviour
{
    
    private PlayerInput inputActions;

    [Header("General Settings")]
    [SerializeField] private EventReference drumSound;
    [SerializeField] private DrumStick drumStick;
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;
    [SerializeField] Interactable interactable;
    [SerializeField] private DrumRadio drumRadio;
    
    [Header("Rhythm Settings")]
    [SerializeField] private float gracePeriod;
    [SerializeField] private int rhythmAmount;
    [SerializeField] private int rhythmLength;
    [SerializeField] private float[] rhythmDelays;
    
    private List<List<float>> rhythms;
    
    private List<float> currentPlayerRhythm;
    private float currentDelay;
    
    private bool inRhythm;
    [HideInInspector] public int currentRhythmIndex;
    [HideInInspector] public bool playRhythm;
    
    [HideInInspector] public bool rhythmMiniGameCompleted;
    
    private void Start()
    {
        rhythms = new List<List<float>>();
        currentPlayerRhythm = new List<float>();
        inRhythm = false;
        currentRhythmIndex = 0;
        playRhythm = false;
        rhythmMiniGameCompleted = false;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        
        for (int i = 0; i < rhythmAmount; i++)
        {
            List<float> rhythm = new List<float>();
            for (int j = 0; j < rhythmLength; j++)
            {
                var delay = 0f;
                
                if (j != 0)
                    delay = rhythmDelays[Random.Range(0, rhythmDelays.Length)];
                
                rhythm.Add(delay);
            }
            rhythms.Add(rhythm);
        }
        
        currentDelay = 0f;
        
    }
    
    private void Update()
    {
        if (interactable.objIsActive && !inRhythm && playRhythm && !rhythmMiniGameCompleted)
        {
            Debug.Log("Playing Radio Rhythm");
            StartCoroutine(PlayRhythm(rhythms[currentRhythmIndex]));
        } 
        else if (interactable.objIsActive && !interactable.isMoving && rhythmMiniGameCompleted)
        {
            inputActions.Drum.Enable();
        }
        
        if (inputActions.Drum.Cancel.triggered && interactable.objIsActive)
        {
            interactable.isMoving = true;
            inputActions.Drum.Disable();
        }
        
        if (inputActions.Drum.BeatDrum.triggered && interactable.objIsActive)
        {
            drumStick.BeatStick(positionTurnSpeed, rotationTurnSpeed);
            AudioManager.Instance.PlayOneShot(drumSound, transform.position);
            currentPlayerRhythm.Add(currentDelay);
            currentDelay = 0f;
            CheckRhythm();
        }
        
        if (interactable.objIsActive && !playRhythm && !rhythmMiniGameCompleted)
        {
            currentDelay += Time.deltaTime;
        }
    }

    private void CheckRhythm()
    {
        if (rhythmMiniGameCompleted)
        {
            return;
        }
        
        if (currentPlayerRhythm.Count > 1 && Math.Abs(currentPlayerRhythm[^1] - rhythms[currentRhythmIndex][currentPlayerRhythm.Count - 1]) > gracePeriod)
        {
            Debug.Log("RYTHM DISCREPANCY: " + currentPlayerRhythm[^1] + " - " + rhythms[currentRhythmIndex][currentPlayerRhythm.Count - 1]);
            inputActions.Drum.Disable();
            Debug.Log("Wrong rhythm");
            currentPlayerRhythm = new List<float>();
            //todo decide whether to restart whole minigame: currentSequenceIndex = 0;
            StartCoroutine(PlayWrongSound());
        }
        if (currentPlayerRhythm.Count == rhythms[currentRhythmIndex].Count)
        {
            Debug.Log("Correct rhythm");
            inputActions.Drum.Disable();
            currentRhythmIndex++;
            if (currentRhythmIndex == rhythms.Count)
            {
                Debug.Log("Finished all rhythms");
                rhythmMiniGameCompleted = true;
                StartCoroutine(PlayFinishedSound());
                return;
            }
            currentPlayerRhythm = new List<float>();
            StartCoroutine(PlayRightSound());
        }
    }
    
    private IEnumerator PlayWrongSound()
    {
        drumRadio.PlayWrongSound();
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayRhythm(rhythms[currentRhythmIndex]));
    }
    
    private IEnumerator PlayRightSound()
    {
        drumRadio.PlayRightSound();
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayRhythm(rhythms[currentRhythmIndex]));
    }
    
    private IEnumerator PlayFinishedSound()
    {
        drumRadio.PlayFinishedSound();
        yield return new WaitForSeconds(1f);
        inputActions.Drum.Enable();
    }


    private IEnumerator PlayRhythm(List<float> rhythm)
    {
        inRhythm = true;
        playRhythm = false;
        inputActions.Drum.Disable();
        foreach (float delay in rhythm)
        { 
           yield return new WaitForSeconds(delay);
           drumRadio.PlayDrumSound();
        }
        inRhythm = false;
        currentPlayerRhythm = new List<float>();
        inputActions.Drum.Enable();
    }
}


