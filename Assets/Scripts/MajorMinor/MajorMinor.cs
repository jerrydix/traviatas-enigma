using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MajorMinor : MonoBehaviour
{
    [SerializeField] private Dictionary<EventReference, bool> melodies;

    [SerializeField] private Dict melodiesDict;
    [SerializeField] private MajorButton majorButton;
    [SerializeField] private MinorButton minorButton;
    private void Start()
    {
        melodies = melodiesDict.toDict();
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
