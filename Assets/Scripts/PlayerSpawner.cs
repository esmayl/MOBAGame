using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public BasicInformation[] championStatArray;
    public GameObject[] champions;
    public GameObject cameraPrefab;

    GameObject cameraInstance;
    string[] names;


    void Awake()
    {
        Vector3 spawnBase = transform.position;

        cameraInstance = Instantiate(cameraPrefab, transform.position, Quaternion.identity);

        int i = 0;

        foreach(BasicInformation info in championStatArray)
        {
            GameObject temp = Instantiate(champions[i]);
            temp.transform.position = spawnBase;
            temp.name = info.name;
            //temp.tag = "Enemy";
            temp.layer = LayerMask.NameToLayer("Attackable");
            temp.GetComponent<Champion>().bi = info;
            temp.GetComponent<Champion>().team = Team.Red;

            temp.GetComponent<ChampionSkills>().Init();

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
