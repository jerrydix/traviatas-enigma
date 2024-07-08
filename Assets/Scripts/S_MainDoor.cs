using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class S_MainDoor : MonoBehaviour
{
    public GameObject[] Lamps;
    public Material on;
    public Material on1;
    public Material on2;
    public Material off;
    private Animator anim;
    public S_Singer singer;
    void Start()
    {
        anim = GetComponent<Animator>();
        ResetLamps();
    }

    public void ResetLamps()
    {
        foreach (GameObject lamp in Lamps)
        {
            List<Material> materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],off};
            lamp.GetComponent<MeshRenderer>().SetMaterials(materials);
        }
    }

    public void TurnLamp(int index, int state)
    {
        GameObject lamp = Lamps[index];
        List<Material> materials;
        switch (state)
        {
            case 0:
                materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],off};
                break;
            case 1:
                materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],on2};
                break;
            case 2:
                materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],on1};
                break;
            case 3:
                materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],on};
                break;
            default:
                materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],off};
                break;
        }
        lamp.GetComponent<MeshRenderer>().SetMaterials(materials);
    }

    public void OpenDoor()
    {
        GameManager.Instance.DisableMusic();
        singer.instance.stop(STOP_MODE.ALLOWFADEOUT);
        anim.Play("Open");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            OpenDoor();
        }
    }
}
