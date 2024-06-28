using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentInventory : MonoBehaviour
{
    [SerializeField] private List<Image> items;
    [SerializeField] private Sprite cymbalSprite;
    [SerializeField] private Sprite trumpetSprite;
    [SerializeField] private Sprite violinSprite;

    [HideInInspector] public List<int> currentItems;

    private Dictionary<Sprite, bool> itemsSetDict;
    
    private int currentItemIndex;

    private void Start()
    {
        itemsSetDict = new Dictionary<Sprite, bool>
        {
            { cymbalSprite, false },
            { trumpetSprite, false },
            { violinSprite, false }
        };
        currentItems = new List<int>();
    }

    public void AddItem(int item)
    {
        switch (item)
        {
            case 0:
                items[currentItemIndex].sprite = cymbalSprite;
                itemsSetDict[cymbalSprite] = true;
                currentItems.Add(0);
                break;
            case 1:
                items[currentItemIndex].sprite = trumpetSprite;
                itemsSetDict[trumpetSprite] = true;
                currentItems.Add(1);
                break;
            case 2:
                items[currentItemIndex].sprite = violinSprite;
                itemsSetDict[violinSprite] = true;
                currentItems.Add(2);
                break;
        }
        
        currentItemIndex++;
    }
    
    public void RemoveItem(int item)
    {
        currentItemIndex--;

        switch (item)
        {
            case 0:
                items[currentItemIndex].sprite = null;
                itemsSetDict[cymbalSprite] = false;
                currentItems.Remove(0);
                break;
            case 1:
                items[currentItemIndex].sprite = null;
                itemsSetDict[trumpetSprite] = false;
                currentItems.Remove(1);
                break;
            case 2:
                items[currentItemIndex].sprite = null;
                itemsSetDict[violinSprite] = false;
                currentItems.Remove(2);
                break;
        }
        
    }

    private void LateUpdate()
    {
        foreach (var image in items)
        {
            if (image.sprite != null && itemsSetDict[image.sprite])
            {
                image.SetNativeSize();
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2(0, 0);
            }
        }
    }
}
