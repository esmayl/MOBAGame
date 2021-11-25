using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public BasicInformation[] championStatArray;
    public GameObject championsObj;
    public GameObject baseChampion;
    public GameObject cameraPrefab;

    GameObject cameraInstance;
    string[] names;


    void Awake()
    {
        //Instantiate(championsObj);

        Vector3 spawnBase = transform.position;

        cameraInstance = Instantiate(cameraPrefab, transform.position, Quaternion.identity);


        foreach(BasicInformation info in championStatArray)
        {
            Vector3 variation = new Vector3(Random.Range(spawnBase.x, 10), 0, Random.Range(spawnBase.z, 10));
            GameObject temp = Instantiate(baseChampion);
            temp.transform.position = spawnBase + variation;
            temp.name = info.name;
            temp.tag = "Enemy";
            temp.layer = LayerMask.NameToLayer("Attackable");
            temp.GetComponent<Champion>().bi = info;
            temp.GetComponent<Champion>().Init();
            temp.GetComponent<Champion>().team = Team.Green;

            if (info.name == "Ahri")
            {
                temp.AddComponent<Player>();
                temp.layer = LayerMask.NameToLayer("Player");
                cameraInstance.GetComponent<PlayerCamera>().playerTransform = temp.transform;
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
