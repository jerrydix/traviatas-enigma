using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

using STOP_MODE = FMOD.Studio.STOP_MODE;

public class S_Radio : MonoBehaviour
{
    private bool toogle;
    [SerializeField] private bool staticRadio;
    [SerializeField] private EventReference radioSound;
    [SerializeField] private EventReference switchSound;

    private EventInstance radioInstance;
    private EventInstance switchInstance;


    private void Start()
    {
        if (staticRadio)
        {
            switchInstance = RuntimeManager.CreateInstance(switchSound);
            RuntimeManager.AttachInstanceToGameObject(switchInstance, transform);
            switchInstance.start();
        }
        else
        {
            radioInstance = RuntimeManager.CreateInstance(radioSound);
            RuntimeManager.AttachInstanceToGameObject(radioInstance, transform);
            radioInstance.start();
        }
    }

    public void TurnOnOff()
    {
        if (toogle)
        {
            if (staticRadio)
            {
                switchInstance.start();
            }
            else
            {
                radioInstance.start();
            }
        }
        else
        {
            if (staticRadio)
            {
                switchInstance.stop(STOP_MODE.IMMEDIATE);
            }
            else
            {
                radioInstance.stop(STOP_MODE.IMMEDIATE);
            }
        }
        toogle = !toogle;
    }
}
