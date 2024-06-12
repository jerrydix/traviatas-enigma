using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Piano : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput inputActions;
    private List<Transform> keys;
    
    [SerializeField] private EventReference[] pianoSounds;
    [SerializeField] private PianoKey[] pianoKeys;
    [SerializeField] private float positionTurnSpeed;
    [SerializeField] private float rotationTurnSpeed;
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
    
    private void PressKey0(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 0 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[0], transform.position);
        pianoKeys[0].PressKey(positionTurnSpeed, rotationTurnSpeed);
    }
    
    private void PressKey1(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 1 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[1], transform.position);
        pianoKeys[1].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey2(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 2 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[2], transform.position);
        pianoKeys[2].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey3(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 3 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[3], transform.position);
        pianoKeys[3].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey4(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 4 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[4], transform.position);
        pianoKeys[4].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
        
    private void PressKey5(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 5 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[5], transform.position);
        pianoKeys[5].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey6(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 6 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[6], transform.position);
        pianoKeys[6].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey7(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 7 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[7], transform.position);
        pianoKeys[7].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey8(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 8 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[8], transform.position);
        pianoKeys[8].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey9(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 9 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[9], transform.position);
        pianoKeys[9].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey10(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 10 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[10], transform.position);
        pianoKeys[10].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PressKey11(InputAction.CallbackContext ctx)
    {
        Debug.Log("Key 11 pressed");
        AudioManager.Instance.PlayOneShot(pianoSounds[11], transform.position);
        pianoKeys[11].PressKey(positionTurnSpeed, rotationTurnSpeed);

    }
    
    private void PlaySound(int key)
    {
        Debug.Log("Playing sound");
    }
}
