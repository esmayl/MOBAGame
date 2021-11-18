using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingProjectile : MonoBehaviour
{
    public Transform targetTransform;
    public Champion owner;
    public float speed;
    public int damage;
    public Team team;

    bool hit = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (owner.dead)
        {
            gameObject.SetActive(false);
        }

        if(targetTransform == null) { return; }
        if (targetTransform.GetComponent<Champion>().dead) { gameObject.SetActive(false); return; }

        transform.LookAt(targetTransform.position);
        transform.Translate(Vector3.forward * (speed * 0.01f) * Time.deltaTime);

        if(Vector3.Distance(transform.position,targetTransform.position) < 1 && !hit)
        {
            hit = true;
            Debug.Log("Hit: " + targetTransform.name);
            targetTransform.GetComponent<Champion>().ChangeHp(damage,owner);
            gameObject.SetActive(false);
        }
    }
}
