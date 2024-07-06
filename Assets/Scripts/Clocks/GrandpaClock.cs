using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GrandpaClock : MonoBehaviour
{
    [SerializeField] private int hour;
    [SerializeField] private int minute; //not too many
    [SerializeField] private EventReference chimeSound;
    private EventInstance instance;
    
    private void Start()
    {
        instance = RuntimeManager.CreateInstance(chimeSound);
        RuntimeManager.AttachInstanceToGameObject(instance, transform);
        instance.start();
    }

    private void Update()
    {
        var _ = (instance.getParameterByName("inMinutes", out float discard1, out float value));
        var _3 = (instance.getParameterByName("Hours", out float discard2,out float hourValue));
        var _2 = (instance.getParameterByName("Minutes", out float discard3,out float minuteValue));
        
        if (hourValue == hour && value == 0)
        {
            instance.setParameterByName("inMinutes", 1);
            instance.setParameterByName("Hours", 0);
            instance.setParameterByName("Minutes", 0);
        }
        
        if (minuteValue == minute && value == 1)
        {
            instance.setParameterByName("inMinutes", 0);
            instance.setParameterByName("Hours", 0);
            instance.setParameterByName("Minutes", 0);
        }
    }
}
