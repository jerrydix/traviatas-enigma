using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private Transform spawn;

    [Header("Movement")]
    private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed = 1f;
    public PlayerInput inputActions;
    [SerializeField] float drag;

    
    [Header("Camera")]
    [SerializeField] private Transform cameraLookAt;
    
    [Header("Sound")]
    [SerializeField] private EventReference tileSteps;
    [SerializeField] private EventReference woodSteps;
    private EventReference steps;

    [SerializeField] private float rate;
    private float time;
   
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        
        inputActions = new PlayerInput();
        inputActions.Moving.Enable();
        inputActions.UI.Enable();
        inputActions.RestartGame.Enable();
        transform.position = spawn.position;
        steps = tileSteps;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Intro")
        {
            GetComponent<S_Effects>().OpenEyes();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveInput = inputActions.Moving.Move.ReadValue<Vector2>();
        Move(moveInput);
    }

    private void Update()
    {
        SpeedLimit();
        _rigidbody.drag = drag;
        time += Time.deltaTime;
        if (inputActions.RestartGame.RestartGame.triggered)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Move(Vector2 input)
    {
        if (input != new Vector2(0,0) && _rigidbody.velocity.magnitude > 0.1f) { 
            if (time > rate)
            {
                AudioManager.Instance.PlayOneShotAttached(steps, gameObject);
                time = 0;
            }
        }
        Vector3 dir = new Vector3(cameraLookAt.forward.x, 0, cameraLookAt.forward.z).normalized * input.y + cameraLookAt.right.normalized * input.x;

        _rigidbody.AddForce(dir * moveSpeed * 10f, ForceMode.Force);
    }
    
    private void SpeedLimit()
    {
        Vector3 vel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        
        if(vel.magnitude > moveSpeed)
        {
            Vector3 lim = vel.normalized * moveSpeed;
            _rigidbody.velocity = new Vector3(lim.x, _rigidbody.velocity.y, lim.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WoodFloor"))
        {
            steps = woodSteps;
        }
        else if (other.gameObject.CompareTag("StoneFloor"))
        {
            steps = tileSteps;
        }
    }
}
