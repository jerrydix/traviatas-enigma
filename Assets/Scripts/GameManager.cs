using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //todo open smth when this is true
    [HideInInspector] public bool pianoMiniGameCompleted;
    [HideInInspector] public bool drumMiniGameCompleted;
    
    private void Start()
    {
        pianoMiniGameCompleted = false;
        drumMiniGameCompleted = false;
    }
    
    
}
