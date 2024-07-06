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
    private Animator anim;
    
    [HideInInspector] public bool motorMiniGameCompleted;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        instance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(instance, transform);
        instance.start();
        checkSequence();
    }

    public void checkSequence()
    {
        if (!motorMiniGameCompleted)
        {
            matchesAmount = 0;
            for (int i = 0; i < sequence.Length; i++)
            {
                currentSequence[i] = levers[i].isToogle;
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
                    anim.Play("Stop");
                    break;
                case 1:
                    instance.setParameterByName("MotorActive", 1);
                    anim.Play("Idle");
                    anim.SetFloat("Speed", 0.1f);
                    break;
                case 2:
                    instance.setParameterByName("MotorActive", 2);
                    anim.Play("Idle");
                    anim.SetFloat("Speed", 0.2f);

                    break;
                case 3:
                    instance.setParameterByName("MotorActive", 3);
                    anim.Play("Idle");
                    anim.SetFloat("Speed", 0.3f);
                    break;
                case 4:
                    instance.setParameterByName("MotorActive", 4);
                    anim.Play("Idle");
                    anim.SetFloat("Speed", 0.4f);
                    break;
                case 5:
                    instance.setParameterByName("MotorActive", 5);
                    anim.Play("Idle");
                    anim.SetFloat("Speed", 1f);
                    break;
            }
            
            if (matchesAmount == sequence.Length)
            {
                motorMiniGameCompleted = true;
                GameManager.Instance.SetMotor();
            }
        }
    }
}
