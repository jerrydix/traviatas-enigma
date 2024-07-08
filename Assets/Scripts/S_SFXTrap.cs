using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_SFXTrap : MonoBehaviour
{
    [SerializeField] private int probability;

    private void OnTriggerEnter(Collider other)
    {
        int rng = Random.Range(1, 100);
        int rng2 = Random.Range(1, 100);
        if (other.CompareTag("Player"))
        {
            if (!GameManager.Instance.isSFXPlaying)
            {
                if (rng2 <= 50)
                {
                    if (rng <= probability)
                    {
                        GameManager.Instance.PlayLong();
                        Debug.Log("Long");
                    }
                }
                else
                {
                    if (rng <= probability)
                    {
                        GameManager.Instance.PlayShort();
                        Debug.Log("Short");

                    }
                }
            }
        }
    }
}
