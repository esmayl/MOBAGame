using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AhriW : SkillState
{
    GameObject shot;

    public AhriW(GameObject player, Champion thisChampion, Animator anim, GameObject shot) : base(player, thisChampion, anim)
    {
        this.shot = shot;
        this.shot.SetActive(false);
    }

    public override void Execute(Transform targetTransform, float deltaTime)
    {

        anim.SetBool("Moving", false);
    }
}
