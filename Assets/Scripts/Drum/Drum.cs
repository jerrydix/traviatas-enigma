using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    
    private PlayerInput inputActions;

    [SerializeField] private DrumStick drumStick;
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;
    [SerializeField] Interactable interactable;
    
    [SerializeField] private int rhythmAmount;
    [SerializeField] private int rhythmLength;
    [SerializeField] private float[] rhythmDelays;
    
    private List<List<float>> rhythms;
    
    private List<float> currentPlayerRhythm;
    private bool inPianoSequence;
    [HideInInspector] public int currentRhythmIndex;
    [HideInInspector] public bool playRhythm;
    
    [HideInInspector] public bool rhythmMiniGameCompleted;
    
    private void Start()
    {
        rhythms = new List<List<float>>();
        currentPlayerRhythm = new List<float>();
        inPianoSequence = false;
        currentRhythmIndex = 0;
        playRhythm = false;
        rhythmMiniGameCompleted = false;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        
        for (int i = 0; i < rhythmAmount; i++)
        {
            List<float> rhythm = new List<float>();
            for (int j = 0; j < rhythmLength; j++)
            {
                rhythm.Add(rhythmDelays[Random.Range(0,rhythmDelays.Length)]);
            }
            rhythms.Add(rhythm);
        }
    }
    
    private void Update()
    {
        if (interactable.objIsActive && !inPianoSequence && playRhythm && !rhythmMiniGameCompleted)
        {
            StartCoroutine(PlayRhythm(rhythms[currentRhythmIndex]));
        } else if (interactable.objIsActive && !interactable.isMoving && rhythmMiniGameCompleted)
        {
            inputActions.Drum.Enable();
        }
        
        if (inputActions.Drum.Cancel.triggered && interactable.objIsActive)
        {
            interactable.isMoving = true;
            inputActions.Drum.Disable();
        }
    }

    private IEnumerator PlayRhythm(List<float> sequence)
    {
        //todo change to drum stuff
        inPianoSequence = true;
        playRhythm = false;
        inputActions.Drum.Disable();
        /*foreach (int key in sequence)
        {
            pianoKeys[key].PressKey(positionTurnSpeed, rotationTurnSpeed);
            AudioManager.Instance.PlayOneShot(pianoSounds[key], transform.position);
            yield return new WaitForSeconds(toneRate);
        }
        inPianoSequence = false;
        currentPlayerSequence = new List<int>();
        inputActions.Drum.Enable();*/
        yield return null;
    }
}


