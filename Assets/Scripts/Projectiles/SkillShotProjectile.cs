using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkillShotProjectile : MonoBehaviour
{
    public float lifetime;
    public float speed = 1;
    public int damage;
    public Team team;
    public Champion owner;

    public bool hit = false;

    float counter = 0;
    Collider[] hits;

    void Update()
    {
        if (counter >= lifetime) { counter = 0; CancelInvoke(); gameObject.SetActive(false); } else { counter += Time.deltaTime; }

        transform.Translate(Vector3.forward * (speed * 0.01f) * Time.deltaTime);

    }

    public void Reset()
    {
        hits = new Collider[1];
        InvokeRepeating("DetectHit", 0, 0.1f);
        GetComponent<VisualEffect>().SendEvent("OnPlay");
    }

    void DetectHit()
    {
        if(Physics.OverlapSphereNonAlloc(transform.position,0.5f,hits) > 0)
        {
            if(Vector3.Distance(hits[0].transform.position ,transform.position) <= 1f)
            {
                if(hits[0].GetComponent<Champion>() && hits[0].GetComponent<Champion>().team != team)
                {
                    Transform hitTransform = hits[0].transform;

                    if (!hit)
                    {
                        hit = true;
                        Debug.Log("Shot Hit: " + hitTransform.name);

                        hitTransform.GetComponent<Champion>().ChangeHp(damage, owner);
                        transform.position = owner.transform.position;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
