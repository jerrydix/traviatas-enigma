using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Elevator : MonoBehaviour
{
    public float positionTurnSpeed;
    private Animator anim;
    private bool isClosed = true;
    private bool isMoving;
    private S_Effects effects;
    [SerializeField] private bool intro;
    private GameObject player;
    public Vector3 teleportPosition = new Vector3(0f, 1f, 0f);
    
    [Header("Buttons")]
    [SerializeField] private ButtonPress insideButton;
    [SerializeField] private ButtonPress outsideButton;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        effects = player.GetComponent<S_Effects>();
    }

    public void InteractInside()
    {
        if (isClosed)
        {
            if (!isMoving && !intro)
            {
                StartCoroutine(Open());
            }
        }
        else
        {
            if (!isMoving && !intro)
            {
                StartCoroutine(Close());
            }
            else if (!isMoving && intro)
            {
                StartCoroutine(Close());
                StartCoroutine(Blink());
            }
        }
    }

    public void InteractOutside()
    {
        if (isClosed)
        {
            outsideButton.PressButton(positionTurnSpeed);
            anim.Play("Open");
            isClosed = false;
        }
    }

    IEnumerator Close()
    {
        isMoving = true;
        insideButton.PressButton(positionTurnSpeed);
        anim.Play("Close");
        yield return new WaitForSeconds(1);
        isMoving = false;
        isClosed = true;
    }
    
    IEnumerator Open()
    {
        isMoving = true;
        insideButton.PressButton(positionTurnSpeed);
        anim.Play("Open");
        yield return new WaitForSeconds(1);
        isMoving = false;
        isClosed = false;
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(2f);
        effects.CloseEyes();
        yield return new WaitForSeconds(effects.effectLength);
        player.transform.position = teleportPosition;
        effects.OpenEyes();
        yield return new WaitForSeconds(effects.effectLength);
    }
}
