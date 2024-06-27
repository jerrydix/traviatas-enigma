using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private EventReference keyPickupSound;
    [SerializeField] private S_Door door;
    void Start()
    {
        
    }

    private void Interact()
    {
        AudioManager.Instance.PlayOneShot(keyPickupSound);
        //todo transfer key to player inventory
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
