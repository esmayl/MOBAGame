using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public BasicInformation[] championStatArray;
    public GameObject baseChampion;
    public GameObject cameraPrefab;

    GameObject cameraInstance;
    string[] names;


    void Awake()
    {
        Vector3 spawnBase = transform.position;

        cameraInstance = Instantiate(cameraPrefab, transform.position, Quaternion.identity);


        foreach(BasicInformation info in championStatArray)
        {
            GameObject temp = Instantiate(baseChampion);
            temp.transform.position = spawnBase;
            temp.name = info.name;
            temp.tag = "Enemy";
            temp.layer = LayerMask.NameToLayer("Attackable");
            temp.GetComponent<Champion>().bi = info;
            temp.GetComponent<Champion>().team = Team.Red;

            if (info.name == "Ahri")
            {
                temp.layer = LayerMask.NameToLayer("Player");
                cameraInstance.GetComponent<PlayerCamera>().playerTransform = temp.transform;

                GetComponent<GameHandler>().thisPlayer = temp.AddComponent<Player>();
                temp.GetComponent<Player>().temporarySpawnSave = spawnBase;

            }

        }


    }
}
