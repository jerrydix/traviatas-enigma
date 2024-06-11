using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piano : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput inputActions;
    private List<Transform> keys;
    void Start()
    {
        keys = new List<Transform>();
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        foreach (Transform key in transform)
        {
            keys.Add(key);
        }

        inputActions.Piano.Key0.performed += PressKey0;
        inputActions.Piano.Key1.performed += PressKey1;
        inputActions.Piano.Key2.performed += PressKey2;
        inputActions.Piano.Key3.performed += PressKey3;
        inputActions.Piano.Key4.performed += PressKey4;
        inputActions.Piano.Key5.performed += PressKey5;
        inputActions.Piano.Key6.performed += PressKey6;
        inputActions.Piano.Key7.performed += PressKey7;
        inputActions.Piano.Key8.performed += PressKey8;
        inputActions.Piano.Key9.performed += PressKey9;
        inputActions.Piano.Key10.performed += PressKey10;
        inputActions.Piano.Key11.performed += PressKey11;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PressKey0(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 0 pressed");
    }
    
    private void PressKey1(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 1 pressed");
    }
    
    private void PressKey2(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 2 pressed");
    }
    
    private void PressKey3(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 3 pressed");
    }
    
    private void PressKey4(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 4 pressed");
    }
        
    private void PressKey5(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 5 pressed");
    }
    
    private void PressKey6(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 6 pressed");
    }
    
    private void PressKey7(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 7 pressed");
    }
    
    private void PressKey8(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 8 pressed");
    }
    
    private void PressKey9(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 9 pressed");
    }
    
    private void PressKey10(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 10 pressed");
    }
    
    private void PressKey11(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 11 pressed");
    }
    
    private void PlaySound(int key)
    {
        Debug.Log("Playing sound");
    }
}
