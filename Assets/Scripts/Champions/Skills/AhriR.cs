using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AhriR : SkillState
{
    GameObject shot;

    public AhriR(GameObject player, Champion thisChampion, Animator anim, GameObject shot) : base(player, thisChampion, anim)
    {
        this.shot = shot;
        this.shot.SetActive(false);
    }

    public override void Execute(Transform targetTransform, float deltaTime)
    {

        anim.SetBool("Moving", false);
    }
}
