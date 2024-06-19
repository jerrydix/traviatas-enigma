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
    [SerializeField] private Dictionary<EventReference, bool> melodies;
    [SerializeField] private int majorMinorAmount;

    [SerializeField] private Dict melodiesDict;
    [SerializeField] private MajorButton majorButton;
    [SerializeField] private MinorButton minorButton;
    [SerializeField] private PlayButton playButton;
    
    private int currentMajorMinorIndex;
    private int currentAmount;
    private EventReference currentMelody;
    private EventInstance currentInstance;
    private bool currentIsMajor;
    
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
            currentInstance.start();
    }
    
    public void MajorButtonPressed()
    {
        if (!majorMinorMiniGameCompleted)
            CheckCorrectness(true);
    }
    
    public void MinorButtonPressed()
    {
        if (!majorMinorMiniGameCompleted)
            CheckCorrectness(false);
    }
    
    private void CheckCorrectness(bool isMajor)
    {
        currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //todo check whether immediate stop is better
        if (currentIsMajor && isMajor || !currentIsMajor && !isMajor)
        {
            Debug.Log("Correct");
            currentAmount++;
            if (currentAmount >= majorMinorAmount)
            {
                //todo finished method
                majorMinorMiniGameCompleted = true;
                Debug.Log("Finished");
            }
            else
            {
                //play next, other melody if correct
                //todo add play right sound
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
            }
        }
        else
        {
            //play other melody if incorrect
            //todo add play wrong sound
            Debug.Log("Incorrect");
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
        }
    }

    private void Update()
    {
        
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
