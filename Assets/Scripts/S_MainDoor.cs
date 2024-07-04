using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MainDoor : MonoBehaviour
{
    public GameObject[] Lamps;
    public Material on;
    public Material off;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        ResetLamps();
        OpenDoor();
    }

    public void ResetLamps()
    {
        foreach (GameObject lamp in Lamps)
        {
            List<Material> materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],off};
            lamp.GetComponent<MeshRenderer>().SetMaterials(materials);
        }
    }

    public void TurnLamp(int index, bool state)
    {
        GameObject lamp = Lamps[index];
        List<Material> materials;
        if (state)
        {
            materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],on};
        }
        else
        {
            materials = new List<Material>{lamp.GetComponent<MeshRenderer>().materials[0],off};
        }
        lamp.GetComponent<MeshRenderer>().SetMaterials(materials);
    }

    public void OpenDoor()
    {
        anim.Play("Open");
    }
}
