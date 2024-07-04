using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PeopleAmbience : MonoBehaviour
{
    [SerializeField] private EventReference ambienceSound;
    void Start()
    {
        AudioManager.Instance.PlayOneShot(ambienceSound, transform.position);
    }
}
