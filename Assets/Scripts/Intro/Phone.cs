using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [SerializeField] private GameObject phoneHandle;
    [SerializeField] Transform phoneDownTransform;
    [SerializeField] private Transform phoneHoldTransform;
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;
    [SerializeField] private EventInstance callInstance;

    [SerializeField] private float transitionDistanceStart;
    [SerializeField] private float transitionDistanceEnd;
    
    private float configuredDistance;
    private float scaledDistance;
    private float phonePlayerDistance;
    private GameObject player;


    [SerializeField] private EventReference callSound;
    [SerializeField] private EventReference phonePickup;
    [SerializeField] private EventReference trans;

    [SerializeField] private S_Effects effects;
    
    
    private PlayerInput inputActions;
    private bool isMoving;
    private bool callIsActive;
    private Coroutine phoneRing;
    private bool isCalling;

    private void Start()    
    {
        isMoving = false;
        callIsActive = false;
        isCalling = true;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        callInstance = RuntimeManager.CreateInstance(callSound);
        RuntimeManager.AttachInstanceToGameObject(callInstance, transform);
        player = GameObject.Find("Player");
        
        //start 5
        //end 1
        //configuredDistance = 4
        //phonePlayerDistance = 2
        //map configuredDistance to 0-1
        
        configuredDistance = transitionDistanceStart - transitionDistanceEnd;
        phonePlayerDistance = Vector3.Distance(transform.position, player.transform.position) - transitionDistanceEnd;
        scaledDistance = 1 - ((phonePlayerDistance - transitionDistanceEnd) / configuredDistance);
        
        StartRing(); //nochekin
    }

    public void Interact()
    {
        if (isCalling)
        {
            callInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            AudioManager.Instance.PlayOneShotAttached(phonePickup, gameObject);
            inputActions.Moving.Disable();
            GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = true;
            isMoving = true;
            StartCoroutine(exitScene());
        }
    }

    IEnumerator exitScene()
    {
        AudioManager.Instance.PlayOneShotAttached(trans, gameObject);
        yield return new WaitForSeconds(4f);
        effects.CloseEyes();
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene("BackroomsMikhail");
    }

    public void StartRing()
    {
        //todo change first sound to ring sound in gamemanager i guess, instead of below thing
        callInstance.start();
    }

    private void Update()
    {
        phonePlayerDistance = Vector3.Distance(transform.position, player.transform.position) - transitionDistanceEnd;
        scaledDistance = 1 - ((phonePlayerDistance - transitionDistanceEnd) / configuredDistance);
        callInstance.setParameterByName("RingParameter", scaledDistance);
        
        if (isMoving && !callIsActive && Vector3.Distance(phoneHandle.transform.position, phoneHoldTransform.position) < 0.005f)
        {
            if (isCalling)
            {
                isCalling = false;
                callIsActive = true;
                isMoving = false;
                StartCoroutine(waitForCallFinish());
            }
        }

        if (isMoving && !callIsActive)
        {
            phoneHandle.transform.position = Vector3.Lerp(phoneHandle.transform.position, phoneHoldTransform.position, positionTurnSpeed * Time.deltaTime);
            phoneHandle.transform.rotation = Quaternion.Lerp(Quaternion.Euler(phoneHandle.transform.rotation.eulerAngles), Quaternion.Euler(phoneHoldTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        }
        else if (isMoving && callIsActive)
        {
            phoneHandle.transform.position = Vector3.Lerp(phoneHandle.transform.position, phoneDownTransform.position, positionTurnSpeed * Time.deltaTime);
            phoneHandle.transform.rotation = Quaternion.Lerp(Quaternion.Euler(phoneHandle.transform.rotation.eulerAngles), Quaternion.Euler(phoneDownTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        }

    }

    IEnumerator waitForCallFinish()
    {
        yield return new WaitForSeconds(1);
    }
    
}
