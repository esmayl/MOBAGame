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

		Vector3 temp = targetPos.position;
		temp.y = player.transform.position.y;

		player.transform.LookAt(temp);
		player.transform.Translate(Vector3.forward * (thisChampion.speed * 0.01f) * deltaTime);
	}
}
