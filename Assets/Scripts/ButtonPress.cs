using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private bool pressed;
    private bool isMoving;

    [SerializeField] private EventReference buttonSound;
    [SerializeField] private Transform pressedTransform;
    
    public Transform originalPosition;
    private Vector3 pressedPosition;
    private float positionTurnSpeed;
    private Vector3 pressedUpdate;

    void Start()
    {
        pressed = false;
        isMoving = false;
        originalPosition = new GameObject().transform;
        originalPosition.parent = transform.parent;
        originalPosition.position = transform.position;
        originalPosition.rotation = transform.rotation;
        pressedPosition = pressedTransform.position;
    }

    public void PressButton(float positionTurnSpeed)
    {
        Debug.Log("Pressed Button");
        AudioManager.Instance.PlayOneShot(buttonSound, transform.position);
        this.positionTurnSpeed = positionTurnSpeed;
        pressed = true;
        isMoving = true;
    }
    
    private void Update()
    {
        if (isMoving && pressed && Vector3.Distance(transform.position, pressedTransform.position) < 0.005f)
        {
            pressed = false;
        }
      
        if (isMoving && pressed)
        {
            transform.position = Vector3.Lerp(transform.position, pressedTransform.position, positionTurnSpeed * Time.deltaTime);
        }
        else if (isMoving && !pressed)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, positionTurnSpeed * Time.deltaTime);
        }
    }
}
