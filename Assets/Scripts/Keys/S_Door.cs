using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class S_Door : MonoBehaviour
{
    private Animator anim;
    public Quaternion openedRotation;
    public Quaternion originalRotation;
    public bool isClosed;
    [SerializeField] private float interSpeed;
    [SerializeField] private int itemIndex;
    public bool lockable;
    [SerializeField] private EventReference keyUnlockSound;
    [HideInInspector] public bool isLocked;
    private float lerpPercent = 0.0f;
    public int rot;
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

        if (Mathf.Abs(originalRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 2)
        {
            isClosed = true;
        }
        else
        {
            isClosed = false;
        }
        
    }

    private void CheckKey()
    {
        if (inventory.currentItems.Contains(itemIndex))
        {
            Debug.Log("Key check");
            isLocked = false;
            AudioManager.Instance.PlayOneShot(keyUnlockSound, transform.position);
            inventory.RemoveItem(itemIndex);
        }
    }
}
