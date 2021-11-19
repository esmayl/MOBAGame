using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion), typeof(HealthBar))]
public class Nexus : MonoBehaviour
{
    Champion thisChampion;

    void Start()
    {
        thisChampion = GetComponent<Champion>();

        thisChampion.bi = Champions.main.GetChampion(gameObject.name);
        thisChampion.Init();
        thisChampion.championDeath += Win;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Win()
    {
		GetComponent<VisualEffect>().SendEvent("Win");
        GetComponent<MeshRenderer>().enabled = false;
    }
}
