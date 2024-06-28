using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    private InstrumentInventory inventory;
    [SerializeField] private int itemIndex;
    
    private void Start()
    {
        inventory = GameObject.Find("InstrumentInventory").GetComponent<InstrumentInventory>();
    }
    
    public void CollectItem()
    {
        inventory.AddItem(itemIndex);
        Destroy(gameObject);
    }
}
