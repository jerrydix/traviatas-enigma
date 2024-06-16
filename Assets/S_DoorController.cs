using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class S_DoorController : MonoBehaviour
{
    [SerializeField] private EventReference[] sounds;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(sounds[0], transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(sounds[1], transform.position);
        }
    }
}
