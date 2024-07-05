using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using Random = UnityEngine.Random;

public class Piano : MonoBehaviour
{
    private PlayerInput inputActions;
    
    [SerializeField] private EventReference[] pianoSounds;
    [SerializeField] private PianoKey[] pianoKeys;
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;
    [SerializeField] Interactable interactable;

    [SerializeField] private int sequenceAmount;
    [SerializeField] private int sequenceLength;
    [SerializeField] private float toneRate;
    //todo add special sequence with theme
    private List<List<int>> sequences;

    private List<int> currentPlayerSequence;
    private bool inPianoSequence;
    [HideInInspector] public int currentSequenceIndex;
    [HideInInspector] public bool playSequence;
    
    [HideInInspector] public bool pianoMiniGameCompleted; 
    
    void Start()
    {
        currentPlayerSequence = new List<int>();
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        
        inputActions.Piano.Key0.performed += PressKey0;
        inputActions.Piano.Key1.performed += PressKey1;
        inputActions.Piano.Key2.performed += PressKey2;
        inputActions.Piano.Key3.performed += PressKey3;
        inputActions.Piano.Key4.performed += PressKey4;
        inputActions.Piano.Key5.performed += PressKey5;
        inputActions.Piano.Key6.performed += PressKey6;
        inputActions.Piano.Key7.performed += PressKey7;
        inputActions.Piano.Key8.performed += PressKey8;
        inputActions.Piano.Key9.performed += PressKey9;
        inputActions.Piano.Key10.performed += PressKey10;
        inputActions.Piano.Key11.performed += PressKey11;
        
        sequences = new List<List<int>>();
        
        for (int i = 0; i < sequenceAmount; i++)
        {
            List<int> sequence = new List<int>();
            for (int j = 0; j < sequenceLength; j++)
            {
                sequence.Add(Random.Range(0, 12));
            }
            sequences.Add(sequence);
        }
    }

    private void Update()
    {
        if (interactable.objIsActive && !inPianoSequence && playSequence && !pianoMiniGameCompleted)
        {
            StartCoroutine(PlaySequence(sequences[currentSequenceIndex]));
        } else if (interactable.objIsActive && !interactable.isMoving && pianoMiniGameCompleted)
        {
            inputActions.Piano.Enable();
        }
        
        if (inputActions.Piano.Cancel.triggered && interactable.objIsActive)
        {
            interactable.isMoving = true;
            inputActions.Piano.Disable();
        }
        
    }
    
    private IEnumerator PlaySequence(List<int> sequence)
    {
        inPianoSequence = true;
        playSequence = false;
        inputActions.Piano.Disable();
        foreach (int key in sequence)
        {
            pianoKeys[key].PressKey(positionTurnSpeed, rotationTurnSpeed);
            AudioManager.Instance.PlayOneShot(pianoSounds[key], transform.position);
            yield return new WaitForSeconds(toneRate);
        }
        inPianoSequence = false;
        currentPlayerSequence = new List<int>();
        inputActions.Piano.Enable();
    }

    private void PressKey0(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[0], transform.position);
        pianoKeys[0].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(0);
        CheckSequence();
    }
    
    private void PressKey1(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[1], transform.position);
        pianoKeys[1].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(1);
        CheckSequence();
    }
    
    private void PressKey2(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[2], transform.position);
        pianoKeys[2].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(2);
        CheckSequence();
    }
    
    private void PressKey3(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[3], transform.position);
        pianoKeys[3].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(3);
        CheckSequence();
    }
    
    private void PressKey4(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[4], transform.position);
        pianoKeys[4].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(4);
        CheckSequence();
    }
        
    private void PressKey5(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[5], transform.position);
        pianoKeys[5].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(5);
        CheckSequence();
    }
    
    private void PressKey6(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[6], transform.position);
        pianoKeys[6].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(6);
        CheckSequence();
    }
    
    private void PressKey7(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[7], transform.position);
        pianoKeys[7].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(7);
        CheckSequence();
    }
    
    private void PressKey8(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[8], transform.position);
        pianoKeys[8].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(8);
        CheckSequence();
    }
    
    private void PressKey9(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[9], transform.position);
        pianoKeys[9].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(9);
        CheckSequence();
    }
    
    private void PressKey10(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[10], transform.position);
        pianoKeys[10].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(10);
        CheckSequence();
    }
    
    private void PressKey11(InputAction.CallbackContext ctx)
    {
        AudioManager.Instance.PlayOneShot(pianoSounds[11], transform.position);
        pianoKeys[11].PressKey(positionTurnSpeed, rotationTurnSpeed);
        currentPlayerSequence.Add(11);
        CheckSequence();
    }
    
    private void CheckSequence()
    {
        if (pianoMiniGameCompleted)
        {
            return;
        }
        if (currentPlayerSequence[^1] != sequences[currentSequenceIndex][currentPlayerSequence.Count - 1])
        {
            inputActions.Piano.Disable();
            Debug.Log("Wrong sequence");
            currentPlayerSequence = new List<int>();
            StartCoroutine(PlayWrongSound());
        }
        Debug.Log(currentPlayerSequence.Count + " " + sequences[currentSequenceIndex].Count);
        if (currentPlayerSequence.Count == sequences[currentSequenceIndex].Count)
        {
            Debug.Log("Correct sequence");
            inputActions.Piano.Disable();
            currentSequenceIndex++;
            if (currentSequenceIndex == sequences.Count)
            {
                Debug.Log("Finished all sequences");
                pianoMiniGameCompleted = true;
                GameManager.Instance.CheckPianos();
                StartCoroutine(PlayFinishedSound());
                return;
            }
            currentPlayerSequence = new List<int>();
            StartCoroutine(PlayRightSound());
        }
    }
    
    private void OnDestroy()
    {
        inputActions.Piano.Key0.performed -= PressKey0;
        inputActions.Piano.Key1.performed -= PressKey1;
        inputActions.Piano.Key2.performed -= PressKey2;
        inputActions.Piano.Key3.performed -= PressKey3;
        inputActions.Piano.Key4.performed -= PressKey4;
        inputActions.Piano.Key5.performed -= PressKey5;
        inputActions.Piano.Key6.performed -= PressKey6;
        inputActions.Piano.Key7.performed -= PressKey7;
        inputActions.Piano.Key8.performed -= PressKey8;
        inputActions.Piano.Key9.performed -= PressKey9;
        inputActions.Piano.Key10.performed -= PressKey10;
        inputActions.Piano.Key11.performed -= PressKey11;
    }
    
    IEnumerator PlayRightSound()
    {
        Debug.Log("Right sequence sound");
        yield return new WaitForSeconds(1);
        StartCoroutine(PlaySequence(sequences[currentSequenceIndex]));
    }
    
    IEnumerator PlayWrongSound()
    {
        Debug.Log("Wrong sequence sound");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < pianoKeys.Length; i++)
        {
            AudioManager.Instance.PlayOneShot(pianoSounds[i], transform.position);
            pianoKeys[i].PressKey(positionTurnSpeed, rotationTurnSpeed);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlaySequence(sequences[currentSequenceIndex]));
    }

    IEnumerator PlayFinishedSound()
    {
        Debug.Log("Finished sequence sound");
        yield return new WaitForSeconds(1);
        foreach (var sequence in sequences)
        {
            foreach (var key in sequence)
            {
                AudioManager.Instance.PlayOneShot(pianoSounds[key], transform.position);
                pianoKeys[key].PressKey(positionTurnSpeed, rotationTurnSpeed);
                yield return new WaitForSeconds(toneRate / 2);
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < pianoKeys.Length; i++)
        {
            AudioManager.Instance.PlayOneShot(pianoSounds[i], transform.position);
            pianoKeys[i].PressKey(positionTurnSpeed, rotationTurnSpeed);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        inputActions.Piano.Enable();
    }
    
    
    
}
