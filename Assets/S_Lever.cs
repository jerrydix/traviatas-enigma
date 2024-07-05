using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class S_Lever : MonoBehaviour
{
    private Animator anim;
    public bool isToogle;
    private bool isMoving;
    [SerializeField] private EventReference toggleSound;
    private S_Motor motor;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        motor = GameObject.FindWithTag("Motor").GetComponent<S_Motor>();
        checkToggle();
    }

    private void checkToggle()
    {
        if (isToogle)
        {
            anim.Play("Open");
        }
        else
        {
            anim.Play("Close");

        }
    }

    public void Toggle()
    {
        if (!isMoving)
        {
            if (isToogle)
            {
                anim.Play("Close");
            }
            else
            {
                anim.Play("Open");
            }
            AudioManager.Instance.PlayOneShotAttached(toggleSound, gameObject);
            StartCoroutine(toggleCor());
            isToogle = !isToogle;
            motor.checkSequence();
        }
    }

    IEnumerator toggleCor()
    {
        isMoving = true;
        yield return new WaitForSeconds(1);
        isMoving = false;
    }
}
