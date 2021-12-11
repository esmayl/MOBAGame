using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState
{
    internal GameObject player;
    internal Animator anim;
    internal Champion thisChampion;
    internal Vector3 targetPosition;
    internal float counter;
    public float cooldown;
    public float castTime;

    public SkillState(GameObject player, Champion thisChampion, Animator anim)
    {
    }

    public virtual void Execute(Transform targetPos, float deltaTime)
    {
        anim.SetBool("Moving", false);
    }

    public virtual bool OnCooldown()
    {
        if(counter < cooldown)
        {
            return true;
        }

        return false;
    }
}
