using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class S_Elevator : MonoBehaviour
{
    public float positionTurnSpeed;
    private Animator anim;
    private bool isClosed = true;
    private bool isMoving;
    private S_Effects effects;
    public GameObject lift;
    
    [SerializeField] private bool intro;
    [SerializeField] private EventReference[] LiftSounds;
    [SerializeField] private Transform doorPosition; 
    
    private GameObject player;
    private EventInstance musicInstance;
    private bool isIn;
    public Vector3 teleportPosition = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector3 originalPos;
    private Vector3 teleportTo;
    private bool onSecondFloor;
    private GameObject cam;
    
    [Header("Buttons")]
    [SerializeField] private ButtonPress insideButton;
    [SerializeField] private ButtonPress outsideButton;
    
    [SerializeField] private GameObject insideButtonGO;
    [SerializeField] private GameObject outsideButtonGO;
    [SerializeField] private bool operaLift;

    private void Start()
    {
        cam = GameObject.FindWithTag("CH");
        originalPos = lift.transform.position;
        musicInstance = RuntimeManager.CreateInstance(LiftSounds[1]);
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        effects = player.GetComponent<S_Effects>();
        teleportTo = teleportPosition;
    }

    public void InteractInside()
    {
        if (isClosed)
        {
            if (!isMoving && !intro && isIn)
            {
                StartCoroutine(Open());
            }
        }
        else
        {
            if (!isMoving && !intro && isIn)
            {
                StartCoroutine(Close());
            }
            else if (!isMoving && intro && isIn)
            {
                StartCoroutine(Close());
                StartCoroutine(Blink());
            }
        }
    }

    public void InteractOutside()
    {
        if (isClosed)
        {
            StartCoroutine(OpenDoors());
        }
    }

    IEnumerator OpenDoors()
    {
        isClosed = false;
        outsideButton.PressButton(positionTurnSpeed);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.PlayOneShot(LiftSounds[0], doorPosition.position);
        anim.Play("Open");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.parent = transform;
            cam.transform.parent = transform;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
            player.transform.parent = null;
            cam.transform.parent = null;

        }
    }

    IEnumerator Close()
    {
        isMoving = true;
        insideButton.PressButton(positionTurnSpeed);
        AudioManager.Instance.PlayOneShot(LiftSounds[2], doorPosition.position);
        anim.Play("Close");
        yield return new WaitForSeconds(2);
        musicInstance.start();
        if (!intro)
        {
            StartCoroutine(LiftSwap());
        }
        else
        {
            isMoving = false;
            isClosed = true;
        }
    }
    
    IEnumerator Open()
    {
        isMoving = true;
        insideButton.PressButton(positionTurnSpeed);
        AudioManager.Instance.PlayOneShot(LiftSounds[0], doorPosition.position);
        anim.Play("Open");
        yield return new WaitForSeconds(2);
        isMoving = false;
        isClosed = false;
        if(operaLift)
        {
            yield return new WaitForSeconds(0.4f);
            GameManager.Instance.DisableMusic();
            AudioManager.Instance.PlayOneShotAttached(LiftSounds[3], gameObject);
        }
        
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(4f);
        effects.CloseEyes();
        yield return new WaitForSeconds(effects.effectLength);
        player.transform.position = teleportPosition;
        StartCoroutine(setMusicParam());
        effects.OpenEyes();
        yield return new WaitForSeconds(effects.effectLength);
    }
    
    IEnumerator LiftSwap()
    {
        StartCoroutine(setMusicParamBack());
        yield return new WaitForSeconds(4f);
        if (onSecondFloor)
        {
            teleportTo = originalPos;
        }
        else
        {
            teleportTo = teleportPosition;
        }

        onSecondFloor = !onSecondFloor;
        lift.transform.position = teleportTo;
        StartCoroutine(setMusicParam());
        yield return new WaitForSeconds(1f);
        StartCoroutine(Open());
    }

    IEnumerator setMusicParam()
    {

        float volume = 0;
        while (volume < 1)
        {
            volume += 0.1f;
            musicInstance.setParameterByName("LiftVolume", volume);
            yield return new WaitForSeconds(0.3f);
        }
        musicInstance.stop(STOP_MODE.IMMEDIATE);
        
    }
    
    IEnumerator setMusicParamBack()
    {

        float volume = 1;
        while (volume >= 0)
        {
            volume -= 0.3f;
            musicInstance.setParameterByName("LiftVolume", volume);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
