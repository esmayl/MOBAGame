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

        thisChampion.Init();
        thisChampion.championDeath += Win;
    }

    void Win()
    {
		GetComponent<VisualEffect>().SendEvent("Win");
        GetComponent<MeshRenderer>().enabled = false;

        GameHandler.instance.SetWonTeam(thisChampion.team);
    }
}
