using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Key : MonoBehaviour
{
    private KeyInventory inventory;

    [SerializeField] private EventReference keyPickupSound;
    [SerializeField] private int itemIndex;
    
    private void Start()
    {
        inventory = GameObject.Find("KeyInventory").GetComponent<KeyInventory>();
    }
    
    public void CollectItem()
    {
        inventory.AddItem(itemIndex);
        AudioManager.Instance.PlayOneShot(keyPickupSound, transform.position);
        Destroy(gameObject);
    }
}
