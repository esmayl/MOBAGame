using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(GameObject player, Champion thisChampion, Animator anim) : base(player, thisChampion, anim)
    {
    }

    public override void Execute(Transform targetPos, float deltaTime)
    {
        anim.SetBool("Moving", false);
    }
}
