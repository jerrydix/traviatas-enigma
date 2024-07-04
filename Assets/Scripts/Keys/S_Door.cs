using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class S_Door : MonoBehaviour
{
    [HideInInspector] public Quaternion openedRotation1;
    [HideInInspector] public Quaternion openedRotation2;
    [SerializeField] private Transform playerSideChecker;
    private bool playerInFront;
    private bool needSideCheck;
    
    [HideInInspector] public Quaternion originalRotation;
    [HideInInspector] public bool isClosed;
    [SerializeField] private float interSpeed;
    [SerializeField] private int itemIndex;
    public bool lockable;
    
    [SerializeField] private EventReference keyUnlockSound;
    [SerializeField] private Transform keyHolePosition;
    
    [SerializeField] private EventReference doorOpenSound;
    [HideInInspector] public bool isLocked;
    private float lerpPercent = 0.0f;
    public int rot;
    private Transform player;
    private KeyInventory inventory;
    
    private bool unlocking;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        originalRotation = transform.rotation;
        
        openedRotation1 = Quaternion.Euler(new Vector3(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y + rot, originalRotation.eulerAngles.z));
        openedRotation2 = Quaternion.Euler(new Vector3(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y - rot, originalRotation.eulerAngles.z));

        inventory = GameObject.Find("KeyInventory").GetComponent<KeyInventory>();
        isLocked = true;
        
        unlocking = false;
        
    }

    private void Update()
    {
        if (Vector3.Distance( transform.parent.transform.parent.position, player.position) <= 2f)
        {
            if (lockable && isLocked && !unlocking)
                CheckKey();
            else if (!lockable || lockable && !isLocked)
            {
                if (needSideCheck)
                {
                    if (Vector3.Distance(playerSideChecker.position, player.position) < Vector3.Distance(transform.position, player.position))
                    {
                        playerInFront = true;
                    }
                    else
                    {
                        playerInFront = false;
                    }   
                }

                if (playerInFront)
                {
                    needSideCheck = false;
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),openedRotation1, interSpeed * Time.deltaTime);
                }
                else
                {
                    needSideCheck = false;
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles),openedRotation2, interSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            Debug.Log("Closing");
            needSideCheck = true;
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
            unlocking = true;
            AudioManager.Instance.PlayOneShot(keyUnlockSound, keyHolePosition.position);
            StartCoroutine(DoorUnlock());
        }
    }
    
    private IEnumerator DoorUnlock()
    {
        yield return new WaitForSeconds(1f);
        inventory.RemoveItem(itemIndex);
        isLocked = false;
        unlocking = false;
        AudioManager.Instance.PlayOneShot(doorOpenSound, transform.position);
    }
}
