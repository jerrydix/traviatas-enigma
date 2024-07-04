using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Bulb : MonoBehaviour
{
    public Material mat;
    private Coroutine coroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        StartBlinking();
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            mat.SetFloat("_Emission", 0f);
            yield return new WaitForSeconds(1);
            mat.SetFloat("_Emission", 1f);
            yield return new WaitForSeconds(1);

        }
    }

    public void StartBlinking()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Blink());
        }
        else
        {
            coroutine = StartCoroutine(Blink());
        }
        
    }

    public void StopBlinking()
    {
        if (coroutine == null)
        {
            StopCoroutine(coroutine);
            mat.SetFloat("_Emission", 0f);
        }
    }

    public void StandBy()
    {
        mat.SetFloat("_Emission", 1f);
    }
}
