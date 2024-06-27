using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
    [SerializeField] private Instrument instrument;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private EventReference interactSound;
    
    //todo if instrument is in players inventory / hand then give it to mannequin upon interaction
    //make UI with 2d sprites for collected instruments and keys
    
    void Start()
    {
        
    }
    
    private void Interact()
    {
        //todo check if player has instrument in hand / inventory if (player has instrument) {
        
        AudioManager.Instance.PlayOneShot(interactSound, transform.position);
        instrument.transform.position = holdPosition.position;
        instrument.gameObject.transform.SetParent(transform);
        
        
    }

    void Update()
    {
        
    }
    
    
}
