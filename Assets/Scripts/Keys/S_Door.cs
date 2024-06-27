using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Door : MonoBehaviour
{
    private Animator anim;
    private Quaternion openedRotation;
    private Quaternion originalRotation;
    [SerializeField] private float interSpeed;
    [SerializeField] private int rot;
    private Transform player;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        originalRotation = transform.rotation;
        openedRotation = Quaternion.Euler(new Vector3(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y + rot, originalRotation.eulerAngles.z));
    }

    private void Update()
    {
        if (Vector3.Distance( transform.parent.transform.parent.position, player.position) <= 2f) //todo add a check for player having the key
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),openedRotation, interSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),originalRotation, interSpeed * Time.deltaTime); 
        }
    }
}
