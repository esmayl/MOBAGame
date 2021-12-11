using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MovementState : PlayerState
{
	public delegate void MovementEvent();

	public event MovementEvent beginMove;
	public event MovementEvent endMove;

    NavMeshAgent thisAgent;
    NavMeshPath path;

    Vector3 lookPos;
    Vector3 normalizedDir;

    //List<Node> path = new List<Node>();
    int currentNode = 0;
    float stopDistance = 0.7f;

	internal MovementState(GameObject player, Champion thisChampion, Animator anim) : base(player, thisChampion, anim)
    {
		this.player = player;
        thisAgent = player.AddComponent<NavMeshAgent>();
        thisAgent.baseOffset = 0.5f;
        thisAgent.radius = 0.7f;
        thisAgent.autoBraking = false;
        thisAgent.autoRepath = false;
        thisAgent.autoTraverseOffMeshLink = false;
        thisAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        path = new NavMeshPath();
    }

    public void RecalculatePath(Vector3 targetPos)
    {
        //path = PathfindingHandler.instance.GetPath(player.transform.position, targetPos, 10, 10);
        if(!thisAgent.CalculatePath(targetPos, path)) 
        {
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(targetPos, out navMeshHit, 10, NavMesh.AllAreas))
            {
                thisAgent.CalculatePath(navMeshHit.position, path);
            }
            else
            {
                Debug.LogError("No path found!");
            }
        }
        currentNode = 1;
        thisAgent.isStopped = false;
    }

	public override void Execute (Transform targetPos,float deltaTime) 
	{
        normalizedDir = (path.corners[currentNode] - player.transform.position).normalized;
        normalizedDir.y = player.transform.position.y;

        lookPos = player.transform.position + normalizedDir;
        lookPos.y = player.transform.position.y;

        beginMove?.Invoke();

        if (Vector3.Distance(player.transform.position, path.corners[currentNode]) > stopDistance)
        {
            player.transform.LookAt(lookPos);
            
            anim.SetBool("Moving", true);

            player.transform.position += normalizedDir * Time.deltaTime * thisChampion.speed * 0.01f;

        }
        else
        {
            if (currentNode < path.corners.Length)
            {
                currentNode++;

            }
        }


		endMove?.Invoke();
	}

    public bool ReachedEnd()
    {
        if(currentNode == path.corners.Length)
        {
            return true;
        }

        return false;
    }

    public void Stop()
    {
        thisAgent.CalculatePath(player.transform.position,path);
        currentNode = 0;
        thisAgent.isStopped = true;
        thisAgent.velocity = Vector3.zero;
    }
}
