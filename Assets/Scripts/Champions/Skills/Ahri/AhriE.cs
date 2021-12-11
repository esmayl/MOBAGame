using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AhriE : SkillState
{
    GameObject shot;

    public AhriE(GameObject player, Champion thisChampion, Animator anim, GameObject shot) : base(player, thisChampion, anim)
    {
        this.shot = shot;
        this.shot.SetActive(false);

        this.anim = anim;

        this.thisChampion = thisChampion;
        this.player = player;

        cooldown = 3;
        counter = cooldown;
        castTime = 1.1f;
    }

    public override void Execute(Transform targetTransform, float deltaTime)
    {
        if(counter < cooldown) { return; }

        anim.SetBool("Moving", false);

        counter = 0;
    }

    public override bool OnCooldown()
    {
        return base.OnCooldown();
    }
}
