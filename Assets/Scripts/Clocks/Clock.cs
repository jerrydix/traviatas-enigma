using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clock : MonoBehaviour
{
    
    [SerializeField] private string hour;
    [SerializeField] private string minute;
    [SerializeField] private EventReference digitSound;
    [SerializeField] private EventReference finishedSound;
    [SerializeField] Interactable interactable;
    
    [SerializeField] private TextMeshProUGUI currentText;
    private int currentCharIndex;
    private List<int> inputList;
    
    private PlayerInput inputActions;

    [HideInInspector] public bool clockIsFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        clockIsFinished = false;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        inputList = new List<int> (new int[4]);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.objIsActive && !interactable.isMoving && clockIsFinished)
        {
            CharacterSelecter(-2);
        }
        
        if (interactable.objIsActive && !interactable.isMoving)
        {
            if (!clockIsFinished)
                CharacterSelecter(currentCharIndex);
            inputActions.Clocks.Digit1.performed += PressKey1;
            inputActions.Clocks.Digit2.performed += PressKey2;
            inputActions.Clocks.Digit3.performed += PressKey3;
            inputActions.Clocks.Digit4.performed += PressKey4;
            inputActions.Clocks.Digit5.performed += PressKey5;
            inputActions.Clocks.Digit6.performed += PressKey6;
            inputActions.Clocks.Digit7.performed += PressKey7;
            inputActions.Clocks.Digit8.performed += PressKey8;
            inputActions.Clocks.Digit9.performed += PressKey9;
            inputActions.Clocks.Digit0.performed += PressKey0;
            inputActions.Clocks.Enable();
        }
        
        if (inputActions.Clocks.Cancel.triggered && interactable.objIsActive)
        {
            if (!clockIsFinished)
                CharacterSelecter(-1);
            interactable.isMoving = true;
            inputActions.Clocks.Digit1.performed -= PressKey1;
            inputActions.Clocks.Digit2.performed -= PressKey2;
            inputActions.Clocks.Digit3.performed -= PressKey3;
            inputActions.Clocks.Digit4.performed -= PressKey4;
            inputActions.Clocks.Digit5.performed -= PressKey5;
            inputActions.Clocks.Digit6.performed -= PressKey6;
            inputActions.Clocks.Digit7.performed -= PressKey7;
            inputActions.Clocks.Digit8.performed -= PressKey8;
            inputActions.Clocks.Digit9.performed -= PressKey9;
            inputActions.Clocks.Digit0.performed -= PressKey0;
            inputActions.Clocks.Disable();
        }
    }

    private void CharacterSelecter(int index)
    {
        
        List<Color> colors = new List<Color> {Color.white, Color.white, Color.white, Color.white};
        switch (index)
        {
            case -2: colors = new List<Color> {Color.green, Color.green, Color.green, Color.green}; break;
            case -1: colors = new List<Color> {Color.white, Color.white, Color.white, Color.white}; break;
            default: colors[index] = Color.red; break;
        }
        
        currentText.SetText(
                    $"{inputList[0].ToString().AddColor(colors[0])}" +
                            $"{inputList[1].ToString().AddColor(colors[1])}" +
                            $"{":".AddColor(Color.white)}" +
                            $"{inputList[2].ToString().AddColor(colors[2])}" +
                            $"{inputList[3].ToString().AddColor(colors[3])}");
    }
    
    private void SetDigit(int digit) {
        inputList[currentCharIndex] = digit;
        currentCharIndex++;
        if (currentCharIndex > 3)
            currentCharIndex = 0;
    }

    private void CheckCorrectness()
    {
        if (inputList[0] == int.Parse(hour[0].ToString()) &&
            inputList[1] == int.Parse(hour[1].ToString()) &&
            inputList[2] == int.Parse(minute[0].ToString()) &&
            inputList[3] == int.Parse(minute[1].ToString()))
        {
            clockIsFinished = true;
            GameManager.Instance.CheckClocks();
            AudioManager.Instance.PlayOneShot(finishedSound, transform.position);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(digitSound, transform.position);
        }
    }

    private void PressKeyLogic(int index)
    {
        if (!clockIsFinished)
        {
            SetDigit(index);
            CheckCorrectness();   
        }
    }
    
    private void PressKey1(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(1);
    }
    
    private void PressKey2(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(2);
    }
    
    private void PressKey3(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(3);
    }
    
    private void PressKey4(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(4);
    }
    
    private void PressKey5(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(5);
    }
    
    private void PressKey6(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(6);
    }
    
    private void PressKey7(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(7);
    }
    
    private void PressKey8(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(8);
    }
    
    private void PressKey9(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(9);
    }
    
    private void PressKey0(InputAction.CallbackContext ctx)
    {
        PressKeyLogic(0);
    }

    private void OnDestroy()
    {
        inputActions.Clocks.Digit1.performed -= PressKey1;
        inputActions.Clocks.Digit2.performed -= PressKey2;
        inputActions.Clocks.Digit3.performed -= PressKey3;
        inputActions.Clocks.Digit4.performed -= PressKey4;
        inputActions.Clocks.Digit5.performed -= PressKey5;
        inputActions.Clocks.Digit6.performed -= PressKey6;
        inputActions.Clocks.Digit7.performed -= PressKey7;
        inputActions.Clocks.Digit8.performed -= PressKey8;
        inputActions.Clocks.Digit9.performed -= PressKey9;
        inputActions.Clocks.Digit0.performed -= PressKey0;
    
    }
}

public static class StringExtensions
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}

