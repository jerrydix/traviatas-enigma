using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private bool pressed;
    private bool isMoving;

    [SerializeField] private Transform pressedTransform;
    
    private Transform originalPosition;
    private Vector3 pressedPosition;
    private float positionTurnSpeed;

    void Start()
    {
        pressed = false;
        isMoving = false;
        originalPosition = new GameObject().transform;
        originalPosition.position = transform.position;
        originalPosition.rotation = transform.rotation;
      
        pressedPosition = pressedTransform.position;
    }

    public void PressButton(float positionTurnSpeed)
    {
        Debug.Log("Pressed Major");
        this.positionTurnSpeed = positionTurnSpeed;
        pressed = true;
        isMoving = true;
    }
    
    private void Update()
    {
      
        if (isMoving && pressed && Vector3.Distance(transform.position, pressedPosition) < 0.005f)
        {
            pressed = false;
        }
      
        if (isMoving && pressed)
        {
            transform.position = Vector3.Lerp(transform.position, pressedPosition, positionTurnSpeed * Time.deltaTime);
        }
        else if (isMoving && !pressed)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, positionTurnSpeed * Time.deltaTime);
        }
    }
}
