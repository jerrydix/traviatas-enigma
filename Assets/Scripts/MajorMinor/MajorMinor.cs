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
    private EventReference currentMelody;
    private EventInstance currentInstance;
    private bool currentIsMajor;
    private bool isPlaying;
    //todo loop melody until major or minor button is pressed
    
    private void Start()
    {
        melodies = melodiesDict.toDict();
        
        var initialIndex = Random.Range(0, melodies.Count);
        currentMajorMinorIndex = initialIndex;
        
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
        currentInstance.start();
    }
    
    public void MajorButtonPressed()
    {
        CheckCorrentness(true);
    }
    
    public void MinorButtonPressed()
    {
        CheckCorrentness(false);
    }
    
    private void CheckCorrentness(bool isMajor)
    {
        currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //todo check whether immediate stop is better
        if (currentIsMajor && isMajor || !currentIsMajor && !isMajor)
        {
            Debug.Log("Correct");
            currentMajorMinorIndex++;
            if (currentMajorMinorIndex >= majorMinorAmount)
            {
                //todo finished method
                Debug.Log("Finished");
            }
            else
            {
                var melody = melodies.Keys.ToArray()[currentMajorMinorIndex];
                currentMelody = melody;
                currentIsMajor = melodies[melody];
            }
        }
        else
        {
            Debug.Log("Wrong");
            //todo play wrong sound and another melody
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
