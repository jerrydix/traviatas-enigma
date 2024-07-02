using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Interactable : MonoBehaviour
{
    [SerializeField] Transform cameraInteractablePosition;
    [SerializeField] private GameObject parentScriptObject;
    
    private Transform cam;
    private Transform cameraOriginalPosition;
    private CameraMove cameraMoveScript;
    private float positionTurnSpeed;
    private float rotationTurnSpeed;
    
    private PlayerInput inputActions;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool objIsActive;
    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        cameraOriginalPosition = GameObject.Find("CameraOriginalPos").transform;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        cameraMoveScript = cam.GetComponent<CameraMove>();
        positionTurnSpeed = 5;
        rotationTurnSpeed = 5;
        isMoving = false;
        objIsActive = false;
    }

    public void Interact()
    {
        switch (gameObject.name)
        {
            case "Piano_1":
                CameraZoomInteractPiano();
                break;
            case "Drum_1":
                CameraZoomInteractDrum();
                break;
            case "Clock_1":
                CameraZoomInteractClock();
                break;
            case "PatephoneButton_1":
                InteractPatephoneButton();
                break;
            case "MajorButton_1":
                InteractMajorButton();
                break;
            case "MinorButton_1":
                InteractMinorButton();
                break;
            case "Phone_1":
                InteractPhone();
                break;
            case "ElevatorButtonInside_1":
                InteractElevatorButtonInside();
                break;
            case "ElevatorButtonOutside_1":
                InteractElevatorButtonOutside();
                break;
            case "Cymbals_1":
                InteractInstrument();
                break;
            case "Trumpet_1":
                InteractInstrument();
                break;
            case "Violin_1":
                InteractInstrument();
                break;
            case "Mann_1_1":
                InteractMann();
                break;
            case "Mann_2_1":
                InteractMann();
                break;
            case "Mann_3_1":
                InteractMann();
                break;
            case "DrumKey_1":
                InteractKey();
                break;
            case "PatephoneKey_1":
                InteractKey();
                break;
            case "PianoKey_1":
                InteractKey();
                break;
            case "Singing_1":
                CameraZoomInteractSinging();
                break;
        }
    }
    
    private void CameraZoomInteractPiano()
    {
        cameraOriginalPosition.position = cam.position;
        cameraOriginalPosition.rotation = cam.rotation;
        cameraOriginalPosition.localScale = cam.localScale;
      
        inputActions.Moving.Disable();
        Piano piano = parentScriptObject.GetComponent<Piano>();
        piano.playSequence = true;
        piano.currentSequenceIndex = 0;
        GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = true;

        cameraMoveScript.enabled = false;
        isMoving = true;
    }

    private void CameraZoomInteractDrum()
    {
        cameraOriginalPosition.position = cam.position;
        cameraOriginalPosition.rotation = cam.rotation;
        cameraOriginalPosition.localScale = cam.localScale;

        inputActions.Moving.Disable();
        Drum drum = parentScriptObject.GetComponent<Drum>();
        drum.playRhythm = true;
        drum.currentRhythmIndex = 0;
        GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = true;

        cameraMoveScript.enabled = false;
        isMoving = true;
    }

    private void CameraZoomInteractClock()
    {
        cameraOriginalPosition.position = cam.position;
        cameraOriginalPosition.rotation = cam.rotation;
        cameraOriginalPosition.localScale = cam.localScale;
        
        inputActions.Moving.Disable();
        GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = true;
        
        cameraMoveScript.enabled = false;
        isMoving = true;
    }
    
    private void InteractPatephoneButton()
    {
        MajorMinor majorMinor = parentScriptObject.GetComponent<MajorMinor>();
        majorMinor.PlayMelody();
    }
    
    private void InteractMajorButton()
    {
        MajorMinor majorMinor = parentScriptObject.GetComponent<MajorMinor>();
        majorMinor.MajorButtonPressed();
    }
    
    private void InteractMinorButton()
    {
        MajorMinor majorMinor = parentScriptObject.GetComponent<MajorMinor>();
        majorMinor.MinorButtonPressed();
    }
    
    private void InteractPhone()
    {
        parentScriptObject.GetComponent<Phone>().Interact();
    }
    
    private void InteractElevatorButtonInside()
    {
        parentScriptObject.GetComponent<S_Elevator>().InteractInside();
    }
    
    private void InteractElevatorButtonOutside()
    {
        parentScriptObject.GetComponent<S_Elevator>().InteractOutside();
    }

    private void InteractInstrument()
    {
        parentScriptObject.GetComponent<Instrument>().CollectItem();
    }
    
    private void InteractMann()
    {
        parentScriptObject.GetComponent<Mannequin>().Interact();
    }
    
    private void InteractKey()
    {
        parentScriptObject.GetComponent<Key>().CollectItem();
    }
    
    private void CameraZoomInteractSinging()
    {
        cameraOriginalPosition.position = cam.position;
        cameraOriginalPosition.rotation = cam.rotation;
        cameraOriginalPosition.localScale = cam.localScale;
        
        inputActions.Moving.Disable();
        GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = true;
        
        cameraMoveScript.enabled = false;
        isMoving = true;
    }

    private void Update()
    { 
      if (isMoving && !objIsActive && Vector3.Distance(cam.transform.position, cameraInteractablePosition.position) < 0.005f)
      {
         isMoving = false;
         Debug.Log("Obj started");
         objIsActive = true;
      }
      
      if (isMoving && objIsActive && Vector3.Distance(cam.transform.position, cameraOriginalPosition.position) < 0.005f)
      {
         isMoving = false;
         objIsActive = false;
         cam.position = cameraOriginalPosition.position;
         cam.rotation = cameraOriginalPosition.rotation;
         cam.localScale = cameraOriginalPosition.localScale;
         
         cameraMoveScript.enabled = true;
         GameObject.Find("Main Camera").GetComponent<Interaction>().inInteraction = false;
         inputActions.Moving.Enable();
      }

      if (isMoving && !objIsActive)
      {
         cam.transform.position = Vector3.Lerp(cam.position, cameraInteractablePosition.position, positionTurnSpeed * Time.deltaTime);
         cam.transform.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(cameraInteractablePosition.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
      }
      else if (isMoving && objIsActive)
      {
         cam.transform.position = Vector3.Lerp(cam.position, cameraOriginalPosition.position, positionTurnSpeed * Time.deltaTime);
         cam.transform.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(cameraOriginalPosition.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
      }
      
    }
    
    
}
