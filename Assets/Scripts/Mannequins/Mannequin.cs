using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
    [SerializeField] private GameObject instrument;
    [SerializeField] private int itemIndex;
    [SerializeField] private EventReference interactSound;
    [HideInInspector] public bool mannequinCompleted;
    private InstrumentInventory inventory;
    
    //todo if instrument is in players inventory / hand then give it to mannequin upon interaction
    //make UI with 2d sprites for collected instruments and keys
    
    void Start()
    {
        mannequinCompleted = false;
        instrument.SetActive(false);
        inventory = GameObject.Find("InstrumentInventory").GetComponent<InstrumentInventory>();
    }
    
    public void Interact()
    {
        if (inventory.currentItems.Contains(itemIndex))
        {
            AudioManager.Instance.PlayOneShot(interactSound, transform.position);
            instrument.SetActive(true);
            mannequinCompleted = true;
            inventory.RemoveItem(itemIndex);
            GameManager.Instance.CheckMannequins();   
        }
    }
}
