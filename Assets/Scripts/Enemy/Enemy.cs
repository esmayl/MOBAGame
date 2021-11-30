using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Enemy : MonoBehaviour
{
    public Transform endPos;
    public int moveStraightCost = 10;
    public int moveDiagonalCost = 10;

    float attackRange = 3f;
    float detectionRange = 7f;
    float expRange = 15;

    NavMeshAgent agent;
    NavMeshPath debuggingPath;

    VisualEffect enemyParticles;
    Vector3 temporarySpawnSave;

    Vector3 targetPosition;
    Transform enemyTransform;
    Animator anim;

    Champion thisChampion;

    AttackState attackState;
    MovementState moveState;

    PlayerState activeState;

    Collider[] hits;
    Collider thisCollider;
    LayerMask layerMask;

    List<Node> path = new List<Node>();
    int currentNode = 0;
    Vector3 direction;

    void Start()
    {
        temporarySpawnSave = transform.position;
        targetPosition = endPos.position;


        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();

        enemyParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        thisChampion.Init();


        thisChampion.championDeath += Respawn;

        attackState = new AttackState(gameObject, thisChampion, anim);
        attackState.endAttack += PlayAttackParticles;
        attackState.enemyDead += EnemyDied;

        moveState = new MovementState(gameObject, thisChampion,anim);
        moveState.beginMove += PlayMovementParticles;

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");

        activeState = moveState;

        path = PathfindingHandler.instance.GetPath(transform.position, endPos.position, moveDiagonalCost, moveStraightCost);

        currentNode = 1;

        direction = path[currentNode].location - transform.position;

    }


    void Update()
    {
        if (thisChampion.dead) { return; }

        attackState.counter += Time.deltaTime;


        if (Vector3.Distance(transform.position, path[currentNode].location) < attackRange)
        {
            if (currentNode < path.Count - 1)
            {
                currentNode++;
                direction = targetPosition - transform.position;
                float angle = Vector3.Angle(transform.forward, direction);

                Vector3 rotation = new Vector3();
                rotation.y = angle;

                transform.Rotate(rotation);
            }
        }

        if (enemyTransform)
        {
            if (enemyTransform.GetComponent<Champion>().dead)
            {
                activeState = moveState;

                enemyTransform = null;

                path = PathfindingHandler.instance.GetPath(transform.position, targetPosition, moveDiagonalCost, moveStraightCost);
                currentNode = 1;
                targetPosition = path[currentNode].location;

            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) < attackRange)
            {
                activeState = attackState;
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) < detectionRange)
            {
                activeState = moveState;

                targetPosition = enemyTransform.position;
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) > detectionRange)
            {
                activeState = moveState;

                enemyTransform = null;

                path = PathfindingHandler.instance.GetPath(transform.position, targetPosition, moveDiagonalCost, moveStraightCost);
                currentNode = 1;
                targetPosition = path[currentNode].location;

            }
        }

        if (!enemyTransform)
        {
            activeState = moveState;

            targetPosition = path[currentNode].location;

            hits = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

            if (hits.Length > 0)
            {

                enemyTransform = Champion.GetClosestEnemy(transform.position, hits, thisCollider, thisChampion.team);
                if (enemyTransform)
                {
                    targetPosition = enemyTransform.position;
                }
            }
        }



        activeState.Execute(enemyTransform, Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > attackRange)
        {
            transform.position += direction * Time.deltaTime;
        }

    }

    void PlayMovementParticles()
    {
        enemyParticles.SendEvent("Walk");
    }

    void PlayAttackParticles()
    {
        enemyParticles.SetVector3("ParticlePos", enemyTransform.position);
        enemyParticles.SendEvent("Play");
    }

    void EnemyDied()
    {
        ResetPathfinding();
    }

    void ResetPathfinding()
    {
        enemyTransform = null;
        path = PathfindingHandler.instance.GetPath(transform.position, endPos.position, moveDiagonalCost, moveStraightCost);
        currentNode = 1;
        targetPosition = path[currentNode].location;
    }

    void Respawn()
    {
        if (thisChampion.dead) { return; }

        thisChampion.dead = true;

        GetComponent<Collider>().enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        Collider[] champsClose = Physics.OverlapSphere(transform.position, expRange);

        foreach(Collider champ in champsClose)
        {
            Champion champComponent =  champ.GetComponent<Champion>();

            if (champComponent)
            {
                if (champComponent.GetComponent<Player>() && champComponent.team != thisChampion.team)
                {
                    Debug.Log(champComponent.name + " Gained: " + thisChampion.bi.expWorth);
                    champComponent.GainExp(thisChampion.bi.expWorth);
                }
            }
        }


        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
        GetComponent<VisualEffect>().SendEvent("Die");

        Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        transform.position = temporarySpawnSave;

        thisChampion.dead = false;

        GetComponent<Collider>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        ResetPathfinding();

        thisChampion.Init();
        activeState = moveState;

        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
    }
}
