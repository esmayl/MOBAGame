using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementState : PlayerState
{
	public delegate void MovementEvent();

	public event MovementEvent beginMove;
	public event MovementEvent endMove;

    List<Node> path = new List<Node>();
    int currentNode = 0;
    float stopDistance = 0.5f;

	internal MovementState(GameObject player, Champion thisChampion, Animator anim) : base(player, thisChampion, anim)
    {
		this.player = player;
    }

    public void RecalculatePath(Vector3 targetPos)
    {
        path = PathfindingHandler.instance.GetPath(player.transform.position, targetPos, 10, 10);
        if(path == null) { Debug.LogError("No path found!"); }
        currentNode = 1;
    }

	public override void Execute (Transform targetPos,float deltaTime) 
	{
		beginMove?.Invoke();

        if (Vector3.Distance(player.transform.position, path[currentNode].location) > stopDistance)
        {
            player.transform.position += (path[currentNode].location - player.transform.position).normalized * Time.deltaTime * thisChampion.speed * 0.01f;

        }
        else
        {
            if (currentNode < path.Count - 1)
            {
                currentNode++;
                player.transform.LookAt(path[currentNode].location);

            }
        }

        anim.SetBool("Moving", true);

		endMove?.Invoke();
	}
}
