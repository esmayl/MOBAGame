using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AttackState : PlayerState 
{
	public delegate void AttackEvent();

	public event AttackEvent beginAttack;
	public event AttackEvent endAttack;
	public event AttackEvent enemyDead;

	internal AttackState(GameObject player, Champion thisChampion, Animator anim) : base(player, thisChampion, anim)
	{
		counter = thisChampion.attackSpeed;
	}

	public override void Execute (Transform targetPos, float deltaTime) 
	{
		if (targetPos == null) { return; }

        Vector3 temp = targetPos.position;
        temp.y = player.transform.position.y;

        player.transform.LookAt(temp);

        anim.SetBool("Moving", false);

        if (counter >= thisChampion.attackSpeed)
        {
            beginAttack?.Invoke();

            if (!targetPos.GetComponent<Champion>().ChangeHp(Mathf.RoundToInt(thisChampion.damage), player.GetComponent<Champion>()))
            {
                anim.SetTrigger("Attack");

                endAttack?.Invoke();
            }
            else
            {
                enemyDead?.Invoke();
            }

            counter = 0;

        }
    }
}
