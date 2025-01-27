using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [HideInInspector] public bool shouldNotPlay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector] public string currentMicrophone; 
    
    private void Start()
    {
        currentMicrophone = null;
        shouldNotPlay = false;
    }
    
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }
    
    public void PlayOneShotAttached(EventReference sound, GameObject obj)
    {
        RuntimeManager.PlayOneShotAttached(sound, obj);
    }
}
