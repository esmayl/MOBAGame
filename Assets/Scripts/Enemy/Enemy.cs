using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Enemy : MonoBehaviour
{
    public float attackRange = 2;
    public float detectionRange = 4;
    public Transform endPos;

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

    void Start()
    {
        temporarySpawnSave = transform.position;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();

        enemyParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        thisChampion.bi = Champions.main.GetChampion(gameObject.name);
        thisChampion.Init();

        agent.speed = thisChampion.speed * 0.03f;

        thisChampion.championDeath += Respawn;

        attackState = new AttackState(gameObject, thisChampion, anim);
        attackState.endAttack += PlayAttackParticles;
        attackState.enemyDead += EnemyDied;

        moveState = new MovementState(gameObject, thisChampion,anim);
        moveState.beginMove += PlayMovementParticles;

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");

        activeState = moveState;
        agent.stoppingDistance = attackRange;
        agent.SetDestination(endPos.position);

    }

    void OnDrawGizmos()
    {
        if (!agent) { return; }
        if (agent.hasPath)
        {
            debuggingPath = agent.path;
            Vector3 previous = transform.position;
            foreach(Vector3 node in debuggingPath.corners)
            {
                Gizmos.DrawLine(previous, node);
                previous = node;
            }
        }
    }

    void Update()
    {
        if (thisChampion.dead) { return; }

        attackState.counter += Time.deltaTime;

        if (!enemyTransform)
        {
            if (agent.destination != endPos.position)
            {
                agent.isStopped = false;
                agent.SetDestination(endPos.position);
                targetPosition = endPos.position;
            }
        }

        hits = Physics.OverlapSphere(gameObject.transform.position, detectionRange, layerMask);

        if(hits.Length > 0 && !enemyTransform)
        {
            enemyTransform = Champion.GetClosestEnemy(transform.position,hits,thisCollider,thisChampion.team);
            if (enemyTransform)
            {
                if (agent.destination != enemyTransform.position)
                {
                    targetPosition = enemyTransform.position;
                    agent.SetDestination(enemyTransform.position);
                }
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) < attackRange && enemyTransform)
        {
            activeState = attackState;
            agent.velocity = Vector3.zero;
        }

        activeState.Execute(enemyTransform, Time.deltaTime);

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
        agent.ResetPath();

        enemyTransform = null;
    }

    void Respawn()
    {
        thisChampion.dead = true;
        GetComponent<Collider>().enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }


        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
        GetComponent<VisualEffect>().SendEvent("Die");

        Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        agent.Warp(temporarySpawnSave);
        thisChampion.dead = false;

        GetComponent<Collider>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        enemyTransform = null;
        targetPosition = endPos.position;

        thisChampion.Init();
        agent.ResetPath();

        agent.SetDestination(targetPosition);
        activeState = moveState;

        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
    }
}
