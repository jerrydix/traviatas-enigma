using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.PlayerLoop;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class S_RoomSpawner : MonoBehaviour
{
    public GameObject roomPrefab;

    [SerializeField] private int roomsAmount;
    [SerializeField] private float offset;
    public EventReference introMusic;
    public EventInstance musicInstance; 
    private bool stopped;
    private GameObject player;
    private float limit;
    
    // Start is called before the first frame update
    void Start()
    {
        stopped = false;
        player = GameObject.FindWithTag("Player").gameObject;
        musicInstance = RuntimeManager.CreateInstance(introMusic);
        limit = roomsAmount * offset / 2;
        Perform();
    }

    private void Perform()
    {
        SpawnRoomUp(transform.position, roomsAmount);
        SpawnRoomDown(transform.position, roomsAmount);
        SpawnRoomRight(transform.position, roomsAmount);
        SpawnRoomLeft(transform.position, roomsAmount);
    }

    public void SpawnRoomLeft(Vector3 pos, int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            Instantiate(roomPrefab, new Vector3(pos.x - offset*i, pos.y, pos.z), Quaternion.identity);
        }
    }
    
    public void SpawnRoomRight(Vector3 pos, int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            Instantiate(roomPrefab, new Vector3(pos.x + offset*i, pos.y, pos.z), Quaternion.identity);
        }
    }
    
    public void SpawnRoomUp(Vector3 pos, int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            Vector3 newPos = new Vector3(pos.x, pos.y, pos.z + offset * i);
            Instantiate(roomPrefab, newPos, Quaternion.identity);
            SpawnRoomRight(newPos, amount);
            SpawnRoomLeft(newPos, amount);
        }
    }
    
    public void SpawnRoomDown(Vector3 pos, int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            Vector3 newPos = new Vector3(pos.x, pos.y, pos.z - offset * i);
            Instantiate(roomPrefab, newPos, Quaternion.identity);
            SpawnRoomRight(newPos, amount);
            SpawnRoomLeft(newPos, amount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioManager.Instance.shouldNotPlay && !stopped)
        {
            stopped = true;
            Debug.Log("Stopped");
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        if (player.transform.position.x > limit)
        {
            player.transform.position = new Vector3(-player.transform.position.x + offset, player.transform.position.y, player.transform.position.z);
        }
        else if(player.transform.position.x < -limit)
        {
            player.transform.position = new Vector3(-player.transform.position.x - offset, player.transform.position.y, player.transform.position.z);
        }
        else if (player.transform.position.z > limit)
        {
            player.transform.position = new Vector3(-player.transform.position.x, player.transform.position.y, -player.transform.position.z + offset);
        }
        else if(player.transform.position.z < -limit)
        {
            player.transform.position = new Vector3(-player.transform.position.x, player.transform.position.y, -player.transform.position.z - offset);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            if (!AudioManager.Instance.shouldNotPlay)
            {
                musicInstance.start(); 
                StartCoroutine(setMusicParam());   
            }
        }
    }

    IEnumerator setMusicParam()
    {
        
        float volume = 0;
        while (volume < 1)
        {
            volume += 0.1f;
            musicInstance.setParameterByName("IntroVolume", volume);
            yield return new WaitForSeconds(0.1f);
        }
        
        float value = 0;
        while (value < 1)
        {
            value += 0.1f;
            musicInstance.setParameterByName("Intro", value);
            yield return new WaitForSeconds(0.8f);
        }

        yield return null;
    }
}
