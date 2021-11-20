using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum Team
{
    Red,
    Green,
    White,
}

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Tower : MonoBehaviour
{
    public float detectionRange = 4;
    public GameObject shotPrefab;

    Champion thisChampion;

    Collider[] hits;
    Collider thisCollider;
    Transform closest;


    int layerMask;

    Transform shotSpawn;
    float counter = 0;


    void Start()
    {
        shotSpawn = transform.Find("ShotSpawn");

        thisChampion = GetComponent<Champion>();
        thisChampion.bi = Champions.main.GetChampion(gameObject.name);
        thisChampion.Init();
        
        counter = thisChampion.attackSpeed;
        thisChampion.championDeath += DestroyTower;
        thisCollider = GetComponent<Collider>();

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");
    }


    void Update()
    {
        if (thisChampion.dead) { return; }

        if (closest != null)
        {
            if (closest.GetComponent<Champion>().dead) { closest = null; }
            else if(Vector3.Distance(transform.position, closest.position) > detectionRange)
            {
                closest = null;
            }


        }

        if (counter >= thisChampion.attackSpeed)
        {
            if (!closest)
            {
                hits = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

                if (hits.Length > 0)
                {
                    closest = Champion.GetClosestEnemy(transform.position, hits, thisCollider, thisChampion.team);
                }
            }
            else
            {
                Attack();
                counter = 0;
            }


        }
        else
        {
            counter += Time.deltaTime;
        }
    }

    void Attack()
    {
        if (!closest) { return; }
        if (closest.GetComponent<Champion>().dead) { closest = null; return; }

        var t = Instantiate(shotPrefab, shotSpawn.position, Quaternion.identity);
        t.GetComponent<FollowingProjectile>().targetTransform = closest.transform;
        t.GetComponent<FollowingProjectile>().speed = thisChampion.speed;
        t.GetComponent<FollowingProjectile>().damage = (int)thisChampion.damage;
        t.GetComponent<FollowingProjectile>().team = thisChampion.team;
        t.GetComponent<FollowingProjectile>().owner = thisChampion;

        if (closest.GetComponent<Champion>().dead) { closest = null; return; }
    }

    void DestroyTower()
    {
        GetComponent<VisualEffect>().SendEvent("Die");
        gameObject.SetActive(false);
    }


}
