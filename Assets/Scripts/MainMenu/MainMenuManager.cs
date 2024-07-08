using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [HideInInspector] public bool isAllowedToMoveCamera;

    [SerializeField] private GameObject menuWrapper;
    [SerializeField] private Transform defaultTransform;
    [SerializeField] private Transform cameraSettingsTransform;
    [SerializeField] private Transform cameraControlsTransform;
    [SerializeField] private Transform cameraPlayTransform;
    [SerializeField] private Transform cam;
    
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;

    private bool inSettings;
    private bool inControls;
    private bool inPlay;
    
    private PlayerInput inputActions;
    [SerializeField] GameObject ui;
    
    void Start()
    {
        isAllowedToMoveCamera = false;
        inSettings = false;
        inControls = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
    }

    public void PlayButton()
    {
        inPlay = true;
    }
    
    public void SettingsButton()
    {
        inSettings = true;
    }
    
    public void ControlsButton()
    {
        inControls = true;
    }
    
    public void BackButton()
    {
        inSettings = false;
        inControls = false;
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }

    void Update()
    {
        
        if (inPlay && Vector3.Distance(cam.transform.position, cameraPlayTransform.position) < 0.005f && !isAllowedToMoveCamera)
        {
            isAllowedToMoveCamera = true;
            inputActions.Moving.Enable();
            inputActions.UI.Enable();
            Cursor.visible = false;
            ui.SetActive(true);
        }
        
        
        if (inSettings && !isAllowedToMoveCamera)
        {
            cam.position = Vector3.Lerp(cam.position, cameraSettingsTransform.position, positionTurnSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(cameraSettingsTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        }
        else if (inControls && !isAllowedToMoveCamera)
        {
            cam.position = Vector3.Lerp(cam.position, cameraControlsTransform.position, positionTurnSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(cameraControlsTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        }
        else if (inPlay && !isAllowedToMoveCamera)
        {
            menuWrapper.SetActive(false);
            cam.position = Vector3.Lerp(cam.position, cameraPlayTransform.position, positionTurnSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(cameraPlayTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        } 
        else if (!isAllowedToMoveCamera)
        {
            cam.position = Vector3.Lerp(cam.position, defaultTransform.position, positionTurnSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Lerp(Quaternion.Euler(cam.rotation.eulerAngles), Quaternion.Euler(defaultTransform.rotation.eulerAngles), rotationTurnSpeed * Time.deltaTime);
        }
    }
}
