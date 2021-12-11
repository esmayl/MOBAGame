using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AhriQ : SkillState
{
    GameObject shot;
    public AhriQ(GameObject player, Champion thisChampion, Animator anim,GameObject shot) : base(player, thisChampion, anim)
    {
        this.shot = shot;
        this.shot.SetActive(false);
        
        this.anim = anim;

        this.thisChampion = thisChampion;
        this.player = player;

        cooldown = 5f;
        counter = cooldown;
        castTime = 1.25f;
        skillDelay = 0.5f;
    }

    public override void Execute(Transform targetTransform, float deltaTime)
    {
        if(counter < cooldown) { return; }


        anim.SetBool("Moving", false);

        anim.SetTrigger("Q");

        counter = 0;

        Shoot();
    }

    async void Shoot()
    {

        await Task.Delay(TimeSpan.FromSeconds(skillDelay));

        SkillShotProjectile temp = shot.GetComponent<SkillShotProjectile>();

        shot.SetActive(true);
        shot.transform.position = player.transform.position + (Vector3.up * 0.5f);
        shot.transform.rotation = player.transform.rotation;

        temp.Reset();
        temp.hit = false;
        temp.speed = 500;
        temp.damage = (int)thisChampion.damage;
        temp.team = player.GetComponent<Champion>().team;
        temp.owner = player.GetComponent<Champion>();
        temp.lifetime = 3f;
    }

    public override bool OnCooldown()
    {
        return base.OnCooldown();
    }
}