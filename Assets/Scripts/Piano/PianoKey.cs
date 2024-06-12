using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
   private bool pressed;
   private bool isMoving;
   
   private Transform originalPosition;
   private Vector3 pressedPosition;
   private Quaternion pressedRotation;
   private float positionTurnSpeed;
   private float rotationTurnSpeed;
   
   private float rot = 5.5f;
   private float yReduction = 0.038f;

   private void Start()
   {
      pressed = false;
      isMoving = false;
      originalPosition = new GameObject().transform;
      originalPosition.position = transform.position;
      originalPosition.rotation = transform.rotation;
      
      pressedPosition = new Vector3(originalPosition.position.x, originalPosition.position.y - yReduction, originalPosition.position.z);
      pressedRotation = Quaternion.Euler(new Vector3(originalPosition.rotation.eulerAngles.x + rot, originalPosition.rotation.eulerAngles.y, originalPosition.rotation.eulerAngles.z));
   }

   public void PressKey(float positionTurnSpeed, float rotationTurnSpeed)
   {
      this.positionTurnSpeed = positionTurnSpeed;
      this.rotationTurnSpeed = rotationTurnSpeed;
      pressed = true;
      isMoving = true;
   }

   private void Update()
   {
      
      if (isMoving && pressed && Vector3.Distance(transform.position, pressedPosition) < 0.005f)
      {
         pressed = false;
         Debug.Log("Key pressed");
      }
      
      if (isMoving && pressed)
      {
         transform.position = Vector3.Lerp(transform.position, pressedPosition, positionTurnSpeed * Time.deltaTime);
         transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), pressedRotation, rotationTurnSpeed * Time.deltaTime);
      }
      else if (isMoving && !pressed)
      {
         transform.position = Vector3.Lerp(transform.position, originalPosition.position, positionTurnSpeed * Time.deltaTime);
         transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), originalPosition.rotation, rotationTurnSpeed * Time.deltaTime);
      }
   }
}
