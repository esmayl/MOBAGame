using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AhriQ : SkillState
{
    GameObject shot;
    public AhriQ(GameObject player, Champion thisChampion, Animator anim,GameObject shot) : base(player, thisChampion, anim)
    {
        this.shot = shot;
        this.shot.SetActive(false);

        cooldown = 5f;
        counter = cooldown;
    }

    public override void Execute(Transform targetTransform, float deltaTime)
    {
        if(counter < cooldown) { return; }

        SkillShotProjectile temp = shot.GetComponent<SkillShotProjectile>();

        counter = 0;

        anim.SetBool("Moving", false);

        shot.SetActive(true);
        shot.transform.position = player.transform.position + (Vector3.up*0.5f);
        shot.transform.rotation = player.transform.rotation;

        temp.Reset();
        temp.hit = false;
        temp.speed = 500;
        temp.damage = (int)thisChampion.damage;
        temp.team = player.GetComponent<Champion>().team;
        temp.owner = player.GetComponent<Champion>();
        temp.lifetime = 3f;
    }
}
