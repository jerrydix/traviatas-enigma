using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorButton : MonoBehaviour
{
    private bool pressed;
    private bool isMoving;

    [SerializeField] private Transform pressedTransform;
    
    private Transform originalPosition;
    private Vector3 pressedPosition;
    private Quaternion pressedRotation;
    private float positionTurnSpeed;

    void Start()
    {
        pressed = false;
        isMoving = false;
        originalPosition = new GameObject().transform;
        originalPosition.position = transform.position;
        originalPosition.rotation = transform.rotation;
      
        pressedPosition = pressedTransform.position;
        pressedRotation = pressedTransform.rotation;
        Debug.Log(originalPosition.position);
        Debug.Log(pressedPosition);
        Debug.Log("");
        Debug.Log(originalPosition.rotation);
        Debug.Log(pressedRotation);
    }
    
    public void PressMinor(float positionTurnSpeed)
    {
        this.positionTurnSpeed = positionTurnSpeed;
        pressed = true;
        isMoving = true;
    }

    private void Update()
    {
      
        if (isMoving && pressed && Vector3.Distance(transform.position, pressedPosition) < 0.001f)
        {
            pressed = false;
        }
      
        if (isMoving && pressed)
        {
            Debug.Log(positionTurnSpeed);
            transform.position = Vector3.Lerp(transform.position, pressedPosition, positionTurnSpeed * Time.deltaTime);
        }
        else if (isMoving && !pressed)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, positionTurnSpeed * Time.deltaTime);
        }
    }
}