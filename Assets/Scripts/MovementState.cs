using UnityEngine;
using System.Collections;

public class MovementState : PlayerState
{
	public delegate void MovementEvent();

	public event MovementEvent beginMove;
	public event MovementEvent endMove;

	float moveCheck = 0.5f;

	internal MovementState(GameObject player, Champion thisChampion, Animator anim) : base(player, thisChampion, anim)
    {
		this.player = player;
    }

	public override void Execute (Transform targetPos,float deltaTime) 
	{
		if (counter >= moveCheck)
		{
			beginMove?.Invoke();

			anim.SetBool("Moving", true);

			endMove?.Invoke();

			counter = 0;
		}
        else
        {
			counter += deltaTime;
        }
	}
}
