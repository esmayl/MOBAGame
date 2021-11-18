using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject championsObj;
    public GameObject baseChampion;
    public GameObject cameraPrefab;
    public Skill[] skillArray;

    GameObject cameraInstance;
    string[] names;


    void Awake()
    {
        Instantiate(championsObj);

        names = new string[]{ "Ahri","Akali","Alistar","Anivia" };
        Vector3 spawnBase = transform.position;

        cameraInstance = Instantiate(cameraPrefab, transform.position, Quaternion.identity);


        foreach(string name in names)
        {
            Vector3 variation = new Vector3(Random.Range(spawnBase.x, 10), 0, Random.Range(spawnBase.z, 10));
            GameObject temp = Instantiate(baseChampion);
            temp.transform.position = spawnBase + variation;
            temp.name = name;
            temp.tag = "Enemy";
            temp.layer = LayerMask.NameToLayer("Attackable");
            temp.GetComponent<Champion>().skillPrefabs = skillArray;
            temp.GetComponent<Champion>().Init();
            temp.GetComponent<Champion>().team = Team.Green;

            if (name == "Ahri")
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
