using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Singer : MonoBehaviour
{
    [SerializeField] private EventReference singer;
    
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(Sing());
    }

    IEnumerator Sing()
    {
        while (true)
        {
            int rnd = Random.Range(30, 120);
            Debug.Log(rnd);
            yield return new WaitForSeconds(rnd);
            PlaySound();
        }
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlayOneShotAttached(singer, gameObject);
    }
}
