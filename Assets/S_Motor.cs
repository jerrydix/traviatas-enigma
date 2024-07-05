using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class S_Motor : MonoBehaviour
{
    [SerializeField] private S_Lever[] levers;
    [SerializeField] private bool[] sequence;
    [SerializeField] private EventReference sound;
    private int matchesAmount;
    private bool[] currentSequence = new []{false, false, false, false, false};
    private EventInstance instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(instance, transform);
        instance.start();
        checkSequence();
    }

    public void checkSequence()
    {
        matchesAmount = 0;
        for (int i = 0; i < sequence.Length; i++)
        {
            currentSequence[i] = levers[i].isToogle;
            Debug.Log(currentSequence[i] + " IS");
        }

        for (int j = 0; j < sequence.Length; j++)
        {
            if (currentSequence[j] == sequence[j])
            {
                matchesAmount++;
            }
        }
        
        switch (matchesAmount)
        {
            case 0:
                instance.setParameterByName("MotorActive", 0);
                break;
            case 1:
                instance.setParameterByName("MotorActive", 1);
                break;
            case 2:
                instance.setParameterByName("MotorActive", 2);
                break;
            case 3:
                instance.setParameterByName("MotorActive", 3);
                break;
            case 4:
                instance.setParameterByName("MotorActive", 4);
                break;
            case 5:
                instance.setParameterByName("MotorActive", 5);
                break;
        }
        
        Debug.Log(matchesAmount);
    }
}
