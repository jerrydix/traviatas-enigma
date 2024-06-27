using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    
    [SerializeField] private float rayLength = 10;
    [SerializeField] private LayerMask layerMaskInteract;
    public bool inInteraction;

    private GameObject hitObj;
    private PlayerInput inputActions;

    private void Start()
    {
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        inInteraction = false;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * rayLength, Color.green);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value) && !inInteraction)
        {
            hitObj = hit.collider.gameObject;
            if (hitObj.CompareTag("Interactable"))
            {
                hitObj.GetComponent<MeshRenderer>().enabled = true;
                if (inputActions.Moving.Interact.triggered)
                {
                    Debug.Log("Interacting with " + hitObj.name);
                    hitObj.GetComponent<Interactable>().Interact();
                }
            }
        }
        else if (hitObj != null)
        {
            hitObj.GetComponent<MeshRenderer>().enabled = false;
            hitObj = null;
        }
    }
}
