using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState
{
    internal GameObject player;
    internal Animator anim;
    internal Champion thisChampion;
    internal Vector3 targetPosition;
    internal float counter;
    internal float cooldown;

    protected PlayerState()
    {
        this.player = null;
        this.thisChampion = null;
    }

    protected PlayerState(GameObject player, Champion thisChampion, Animator anim)
    {
        this.player = player;
        this.thisChampion = thisChampion;
        this.anim = anim;
    }

    public virtual void Execute(Transform targetPos, float deltaTime)
    {
        targetPos.position = new Vector3(0, 0, 0);
    }
}
