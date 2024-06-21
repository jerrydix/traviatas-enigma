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
    [SerializeField] private EventReference rightSound;
    [SerializeField] private EventReference wrongSound;
    [SerializeField] private EventReference finishedSound;
    
    [SerializeField] float positionTurnSpeed;
    [SerializeField] float rotationTurnSpeed;
    
    [Header("Buttons")]
    [SerializeField] private MajorButton majorButton;
    [SerializeField] private MinorButton minorButton;
    [SerializeField] private PlayButton playButton;
    
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
        
        var initialIndex = Random.Range(0, melodies.Count);
        currentMajorMinorIndex = initialIndex;

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
        currentInstance.set3DAttributes(gameObject.transform.position.To3DAttributes());
    }
    
    public void PlayMelody()
    {
        if (!majorMinorMiniGameCompleted)
        {
            currentInstance.start();
            isPlaying = true;
        }
    }
    
    public void MajorButtonPressed()
    {
        majorButton.PressMajor(positionTurnSpeed);
        if (!majorMinorMiniGameCompleted && isPlaying)
        {
            CheckCorrectness(true);   
        }
    }
    
    public void MinorButtonPressed()
    {
        minorButton.PressMinor(positionTurnSpeed);
        if (!majorMinorMiniGameCompleted && isPlaying)
        {
            CheckCorrectness(false);   
        }
    }
    
    private void CheckCorrectness(bool isMajor)
    {
        currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //todo check whether immediate stop is better
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
                var newIndex = Random.Range(0, melodies.Count);
                while (newIndex == currentMajorMinorIndex)
                {
                    newIndex = Random.Range(0, melodies.Count);
                }
                currentMajorMinorIndex = newIndex;
                
                var melody = melodies.Keys.ToArray()[currentMajorMinorIndex];
                currentMelody = melody;
                currentIsMajor = melodies[melody];
                currentInstance = RuntimeManager.CreateInstance(currentMelody);
                currentInstance.set3DAttributes(gameObject.transform.position.To3DAttributes());
                currentInstance.start();
                isPlaying = true;
            }
        }
        else
        {
            //play other melody if incorrect
            Debug.Log("Incorrect");
            StartCoroutine(PlayWrongSound());
            var newIndex = Random.Range(0, melodies.Count);
            while (newIndex == currentMajorMinorIndex)
            {
                newIndex = Random.Range(0, melodies.Count);
            }
            currentMajorMinorIndex = newIndex;
                
            var melody = melodies.Keys.ToArray()[currentMajorMinorIndex];
            currentMelody = melody;
            currentIsMajor = melodies[melody];
            currentInstance = RuntimeManager.CreateInstance(currentMelody);
            currentInstance.set3DAttributes(gameObject.transform.position.To3DAttributes());
            currentInstance.start();
            isPlaying = true;
        }
    }

    IEnumerator PlayRightSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot(rightSound, transform.position);
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator PlayWrongSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot(wrongSound, transform.position);
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator PlayFinishedSound()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot(finishedSound, transform.position);
        yield return new WaitForSeconds(1f);
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
