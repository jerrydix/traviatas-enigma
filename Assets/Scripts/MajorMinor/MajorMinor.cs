using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class MajorMinor : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int majorMinorAmount;
    [SerializeField] private Dict melodiesDict;
    private Dictionary<EventReference, bool> melodies;
    private List<EventReference> melodyList;
    private static System.Random rng;
        
    [SerializeField] private EventReference rightSound;
    [SerializeField] private EventReference wrongSound;
    [SerializeField] private EventReference finishedSound;
    [SerializeField] private Transform soundPosition;
    
    [SerializeField] float positionTurnSpeed;
    [SerializeField] float rotationTurnSpeed;
    
    [Header("Buttons")]
    [SerializeField] private ButtonPress majorButton;
    [SerializeField] private ButtonPress minorButton;
    [SerializeField] private ButtonPress playButton;
    
    private int currentMajorMinorIndex;
    private int currentAmount;
    private EventReference currentMelody;
    private EventInstance currentInstance;
    private bool currentIsMajor;

    private bool isPlaying;
    
    [HideInInspector] public bool majorMinorMiniGameCompleted;
    
    private void Start()
    {
        melodies = melodiesDict.toDict();
        majorMinorMiniGameCompleted = false;

        melodyList = melodies.Keys.ToList();
        Shuffle(melodyList);
        
        currentMajorMinorIndex = 0;
        currentAmount = 0;
        
        if (melodies.Count == 0)
        {
            Debug.LogError("No melodies in the dictionary");
            return;
        }
        
        var melody = melodies.Keys.ToArray()[currentMajorMinorIndex];
        currentMelody = melody;
        currentIsMajor = melodies[melody];
        currentInstance = RuntimeManager.CreateInstance(currentMelody);
        RuntimeManager.AttachInstanceToGameObject(currentInstance, soundPosition);
    }
    
    private static void Shuffle<T>(IList<T> list)
    {
        rng = new System.Random();
        var n = list.Count;  
        while (n > 1) {  
            n--;  
            var k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
    
    public void PlayMelody()
    {
        playButton.PressButton(positionTurnSpeed);
        if (!majorMinorMiniGameCompleted)
        {
            currentInstance.start();
            isPlaying = true;
        }
    }
    
    public void MajorButtonPressed()
    {
        majorButton.PressButton(positionTurnSpeed);
        if (!majorMinorMiniGameCompleted && isPlaying)
        {
            CheckCorrectness(true);   
        }
    }
    
    public void MinorButtonPressed()
    {
        minorButton.PressButton(positionTurnSpeed);
        if (!majorMinorMiniGameCompleted && isPlaying)
        {
            CheckCorrectness(false);   
        }
    }
    
    private void CheckCorrectness(bool isMajor)
    {
        currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isPlaying = false;
        if (currentIsMajor && isMajor || !currentIsMajor && !isMajor)
        {
            currentAmount++;
            if (currentAmount >= majorMinorAmount)
            {
                Debug.Log("Finished");
                majorMinorMiniGameCompleted = true;
                StartCoroutine(PlayFinishedSound());
            }
            else
            {
                //play next, other melody if correct
                Debug.Log("Correct");
                StartCoroutine(PlayRightSound());
            }
        }
        else
        {
            //play other melody if incorrect
            Debug.Log("Incorrect");
            StartCoroutine(PlayWrongSound());
        }
    }

    IEnumerator PlayRightSound()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlayOneShot(rightSound, soundPosition.position);
        yield return new WaitForSeconds(0.5f);
        currentMajorMinorIndex++;
        if (currentMajorMinorIndex >= melodyList.Count)
        {
            currentMajorMinorIndex = 0;
            Shuffle(melodyList);
        }
                
        var melody = melodyList[currentMajorMinorIndex];
        currentMelody = melody;
        currentIsMajor = melodies[melody];
        currentInstance = RuntimeManager.CreateInstance(currentMelody);
        RuntimeManager.AttachInstanceToGameObject(currentInstance, soundPosition);
        currentInstance.start();
        isPlaying = true;
    }
    
    IEnumerator PlayWrongSound()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlayOneShot(wrongSound, soundPosition.position);
        yield return new WaitForSeconds(0.5f);
        currentMajorMinorIndex++;
        if (currentMajorMinorIndex >= melodyList.Count)
        {
            currentMajorMinorIndex = 0;
            Shuffle(melodyList);
        }
                
        var melody = melodyList[currentMajorMinorIndex];
        currentMelody = melody;
        currentIsMajor = melodies[melody];
        currentInstance = RuntimeManager.CreateInstance(currentMelody);
        RuntimeManager.AttachInstanceToGameObject(currentInstance, soundPosition);
        currentInstance.start();
        isPlaying = true;
    }
    
    IEnumerator PlayFinishedSound()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlayOneShot(finishedSound, soundPosition.position);
    }
}


[Serializable]
public class Dict
{
    [SerializeField] DictItem[] melodies;

    public Dictionary<EventReference, bool> toDict()
    {
        var dict = new Dictionary<EventReference, bool>();
        foreach (var item in melodies)
        {
            dict.Add(item.melody, item.isMajor);
        }
        
        return dict;
    }
}

[Serializable]
public class DictItem
{
    public EventReference melody;
    public bool isMajor;
}
