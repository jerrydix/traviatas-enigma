using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Room : MonoBehaviour
{
    public GameObject door;
    public S_Door doorScript;
    private bool doorClosed;
    private bool isIn;
    public GameObject[] rooms;
    private GameObject player;
    private GameObject CameraHolder;
    private bool swapped;
    private Coroutine swapCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        CameraHolder = GameObject.FindWithTag("CH");
        player = GameObject.FindWithTag("Player");
        doorScript = door.GetComponent<S_Door>();
    }

    IEnumerator startSwap()
    {
        yield return new WaitForSeconds(1);
        if (doorClosed && isIn)
        {
            swap(); 
        }
    }

    public void swap()
    {
        swapped = true;
        GameObject swapRoom = rooms[Random.Range(0, rooms.Length)];
        S_Room swapRoomScript = swapRoom.GetComponent<S_Room>();
        Vector3 originalPos = transform.position;
        Quaternion originalRotation = transform.rotation;
        Vector3 newPos = swapRoom.transform.position;
        Quaternion newRotation = swapRoom.transform.rotation;

        transform.position = newPos;
        transform.rotation = newRotation;
        
        swapRoom.transform.position = originalPos;
        swapRoom.transform.rotation = originalRotation;
        
        swapRoomScript.doorScript.originalRotation = swapRoomScript.door.transform.rotation;
        swapRoomScript.doorScript.openedRotation1 = Quaternion.Euler(new Vector3(swapRoomScript.doorScript.originalRotation.eulerAngles.x, swapRoomScript.doorScript.originalRotation.eulerAngles.y + swapRoomScript.doorScript.rot, swapRoomScript.doorScript.originalRotation.eulerAngles.z));
        swapRoomScript.doorScript.openedRotation2 = Quaternion.Euler(new Vector3(swapRoomScript.doorScript.originalRotation.eulerAngles.x, swapRoomScript.doorScript.originalRotation.eulerAngles.y - swapRoomScript.doorScript.rot, swapRoomScript.doorScript.originalRotation.eulerAngles.z));
        
        doorScript.originalRotation = door.transform.rotation;
        doorScript.openedRotation1 = Quaternion.Euler(new Vector3(doorScript.originalRotation.eulerAngles.x, doorScript.originalRotation.eulerAngles.y + doorScript.rot, doorScript.originalRotation.eulerAngles.z));
    }

    private void Update()
    {
        doorClosed = doorScript.isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
            player.transform.parent = transform;
            CameraHolder.transform.parent = transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && doorClosed && swapped == false && swapCoroutine == null)
        {
            swapCoroutine = StartCoroutine(startSwap());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
            swapped = false;
            player.transform.parent = null;
            CameraHolder.transform.parent = null;
            swapCoroutine = null;
        }
    }
}
