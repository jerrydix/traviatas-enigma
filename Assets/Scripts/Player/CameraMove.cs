using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    
    private PlayerInput inputActions;
    
    private float xRotation;
    private float yRotation;
    
    public float xSensi;
    public float ySensi;

    private GameObject _pause;

    private void Start()
    {
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      
        _pause = GameObject.Find("UI");
    }

    void Update()
    {
        
        float x = inputActions.Moving.Look.ReadValue<Vector2>().x * Time.deltaTime * xSensi;
        float y = inputActions.Moving.Look.ReadValue<Vector2>().y * Time.deltaTime * ySensi;
        
        yRotation += x;
        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        
        orientation.rotation = Quaternion.Euler(orientation.rotation.x, transform.rotation.eulerAngles.y, orientation.rotation.y);
    }
}
