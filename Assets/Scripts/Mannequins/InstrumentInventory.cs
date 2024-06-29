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
                ItemRemover(0);
                itemsSetDict[cymbalSprite] = false;
                break;
            case 1:
                ItemRemover(1);
                itemsSetDict[trumpetSprite] = false;
                break;
            case 2:
                ItemRemover(2);
                itemsSetDict[violinSprite] = false;
                break;
        }
    }

    private void ItemRemover(int item)
    {
        items[currentItems.IndexOf(item)].sprite = null;
        for (int i = currentItems.IndexOf(item) + 1; i < items.Count; i++)
        {
            items[i - 1].sprite = items[i].sprite;
            if (i == items.Count - 1)
                items[i].sprite = null;
            if (i < currentItems.Count)
                currentItems[i - 1] = currentItems[i];
        }
        currentItems.Remove(item);
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
