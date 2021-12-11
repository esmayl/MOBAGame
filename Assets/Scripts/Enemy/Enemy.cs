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

    VisualEffect enemyParticles;
    Vector3 temporarySpawnSave;

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
        //moveState.beginMove += PlayMovementParticles;

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");

        activeState = moveState;

        moveState.RecalculatePath(endPos.position);

        enemyTransform = null;
    }


    void Update()
    {
        if (thisChampion.dead) { return; }

        attackState.counter += Time.deltaTime;

        if (!enemyTransform)
        {
            activeState = moveState;

            hits = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

            if (hits.Length > 0)
            {

                enemyTransform = Champion.GetClosestEnemy(transform.position, hits, thisCollider, thisChampion.team);
                if (enemyTransform != null)
                {
                    moveState.RecalculatePath(enemyTransform.position);
                }
            }
        }

        if (enemyTransform)
        {

            if (enemyTransform.GetComponent<Champion>().dead)
            {
                activeState = moveState;

                enemyTransform = null;
                moveState.RecalculatePath(endPos.position);
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) < attackRange)
            {
                activeState = attackState;
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) < detectionRange)
            {
                activeState = moveState;
            }
            else if (Vector3.Distance(transform.position, enemyTransform.position) > detectionRange*2)
            {
                activeState = moveState;
                enemyTransform = null;
                moveState.RecalculatePath(endPos.position);
            }
        }

        activeState.Execute(enemyTransform, Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,255,0,0.1f);

        Gizmos.DrawSphere(transform.position, detectionRange);
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
        moveState.RecalculatePath(endPos.position);
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

        DistributeExp();

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

    void DistributeExp()
    {
        Collider[] champsClose = Physics.OverlapSphere(transform.position, expRange);

        foreach (Collider champ in champsClose)
        {
            Champion champComponent = champ.GetComponent<Champion>();

            if (champComponent)
            {
                if (champComponent.GetComponent<Player>() && champComponent.team != thisChampion.team)
                {
                    champComponent.GainExp(thisChampion.bi.expWorth);
                }
            }
        }
    }

}
