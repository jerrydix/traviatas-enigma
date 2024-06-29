using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class S_Door : MonoBehaviour
{
    private Animator anim;
    private Quaternion openedRotation;
    private Quaternion originalRotation;
    [SerializeField] private float interSpeed;
    [SerializeField] private int rot;
    [SerializeField] private int itemIndex;
    public bool lockable;
    [SerializeField] private EventReference keyUnlockSound;
    [HideInInspector] public bool isLocked;
    private Transform player;
    private KeyInventory inventory;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").transform;
        originalRotation = transform.rotation;
        openedRotation = Quaternion.Euler(new Vector3(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y + rot, originalRotation.eulerAngles.z));
        inventory = GameObject.Find("KeyInventory").GetComponent<KeyInventory>();
        isLocked = true;
    }

    private void Update()
    {
        if (Vector3.Distance( transform.parent.transform.parent.position, player.position) <= 2f)
        {
            if (lockable && isLocked)
                CheckKey();
            else
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),openedRotation, interSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),originalRotation, interSpeed * Time.deltaTime); 
        }
    }

    private void CheckKey()
    {
        if (inventory.currentItems.Contains(itemIndex))
        {
            isLocked = false;
            AudioManager.Instance.PlayOneShot(keyUnlockSound, transform.position);
            inventory.RemoveItem(itemIndex);
        }
    }
}
