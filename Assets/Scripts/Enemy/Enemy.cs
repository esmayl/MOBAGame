using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Enemy : MonoBehaviour
{
    public float expRange = 15;
    public Transform endPos;

    float attackRange = 3f;
    float detectionRange = 7f;

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
        thisChampion.Init();

        agent.speed = thisChampion.speed * 0.03f;
        agent.stoppingDistance = attackRange*0.9f;
        agent.SetDestination(endPos.position);

        thisChampion.championDeath += Respawn;

        attackState = new AttackState(gameObject, thisChampion, anim);
        attackState.endAttack += PlayAttackParticles;
        attackState.enemyDead += EnemyDied;

        moveState = new MovementState(gameObject, thisChampion,anim);
        moveState.beginMove += PlayMovementParticles;

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");

        activeState = moveState;


    }


    void Update()
    {
        if (thisChampion.dead) { return; }

        attackState.counter += Time.deltaTime;

        if (enemyTransform)
        {
            if (enemyTransform.GetComponent<Champion>().dead)
            {
                activeState = moveState;

                enemyTransform = null;
                targetPosition = endPos.position;
                agent.isStopped = false;

                agent.SetDestination(targetPosition);
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) < attackRange)
            {
                activeState = attackState;
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
            }
            else if(Vector3.Distance(transform.position, enemyTransform.position) < detectionRange)
            {
                activeState = moveState;
                agent.isStopped = false;

                targetPosition = enemyTransform.position;

                agent.SetDestination(enemyTransform.position);
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) > detectionRange)
            {
                activeState = moveState;

                enemyTransform = null;
                targetPosition = endPos.position;
                agent.isStopped = false;

                agent.SetDestination(targetPosition);
            }
        }

        if (!enemyTransform)
        {
            activeState = moveState;

            if (agent.isStopped)
            {

                enemyTransform = null;
                targetPosition = endPos.position;
                agent.isStopped = false;

                agent.SetDestination(targetPosition);
            }

            hits = Physics.OverlapSphere(gameObject.transform.position, detectionRange, layerMask);

            enemyTransform = Champion.GetClosestEnemy(transform.position, hits, thisCollider, thisChampion.team);
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
