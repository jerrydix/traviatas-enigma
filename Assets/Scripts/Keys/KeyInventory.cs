using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    [SerializeField] private List<Image> items;
    [SerializeField] private Sprite drumKeySprite;
    [SerializeField] private Sprite pianoKeySprite;
    [SerializeField] private Sprite patephoneKeySprite;

    [HideInInspector] public List<int> currentItems;
    private Dictionary<Sprite, bool> itemsSetDict;
    private int currentItemIndex;

    private void Start()
    {
        itemsSetDict = new Dictionary<Sprite, bool>
        {
            { drumKeySprite, false },
            { pianoKeySprite,  false },
            { patephoneKeySprite, false }
        };
        currentItems = new List<int>();
    }

    public void AddItem(int item)
    {
        switch (item)
        {
            case 0:
                items[currentItemIndex].sprite = drumKeySprite;
                itemsSetDict[drumKeySprite] = true;
                currentItems.Add(0);
                break;
            case 1:
                items[currentItemIndex].sprite = pianoKeySprite;
                itemsSetDict[pianoKeySprite] = true;
                currentItems.Add(1);
                break;
            case 2:
                items[currentItemIndex].sprite = patephoneKeySprite;
                itemsSetDict[patephoneKeySprite] = true;
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
                itemsSetDict[drumKeySprite] = false;
                break;
            case 1:
                ItemRemover(1);
                itemsSetDict[pianoKeySprite] = false;
                break;
            case 2:
                ItemRemover(2);
                itemsSetDict[patephoneKeySprite] = false;
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
