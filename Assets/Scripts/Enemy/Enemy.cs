using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Enemy : MonoBehaviour
{
    public float detectionRange = 4;
    public Transform endPos;

    NavMeshAgent agent;
    NavMeshPath resultPath;
    int currentPathNode = 0;

    VisualEffect enemyParticles;
    Vector3 temporarySpawnSave;

    Vector3 targetPosition;
    Transform currentTransform;
    Transform fakeTransform;
    Transform enemyTransform;
    Animator anim;

    Champion thisChampion;

    AttackState attackState;
    MovementState moveState;
    IdleState idleState;

    PlayerState activeState;

    Collider[] hits;
    Collider thisCollider;
    LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        fakeTransform = new GameObject().transform;
        fakeTransform.name = "Fake pos";

        currentTransform = fakeTransform;

        temporarySpawnSave = transform.position;
        agent = GetComponent<NavMeshAgent>();
        resultPath = new NavMeshPath();
        anim = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();

        enemyParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        thisChampion.bi = Champions.main.GetChampion(gameObject.name);
        thisChampion.Init();
        thisChampion.championDeath += Respawn;

        attackState = new AttackState(gameObject, thisChampion, anim);
        //attackState.beginAttack += CheckIfEnemyAlive;
        attackState.endAttack += PlayAttackParticles;
        attackState.enemyDead += CheckIfEnemyAlive;

        moveState = new MovementState(gameObject, thisChampion,anim);
        moveState.beginMove += PlayMovementParticles;

        idleState = new IdleState(gameObject, thisChampion, anim);

        activeState = idleState;

        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Attackable");

    }

    void Update()
    {
        if (thisChampion.dead) { return; }

        attackState.counter += Time.deltaTime;

        if (!enemyTransform)
        {
            if (agent.CalculatePath(endPos.position, resultPath))
            {
                currentPathNode = 0;
                fakeTransform.position = resultPath.corners[currentPathNode];
                currentTransform = fakeTransform;
                targetPosition = fakeTransform.position;
            }
        }

        hits = Physics.OverlapSphere(gameObject.transform.position, detectionRange, layerMask);

        if(hits.Length > 0)
        {
            enemyTransform = Champion.GetClosestEnemy(transform.position,hits,thisCollider,thisChampion.team);
            if (enemyTransform)
            {
                if (agent.CalculatePath(enemyTransform.position, resultPath))
                {
                    if (resultPath.corners.Length > 1)
                    {
                        currentPathNode = 0;
                        fakeTransform.position = resultPath.corners[currentPathNode];
                        currentTransform = fakeTransform;
                        targetPosition = fakeTransform.position;
                    }
                    else
                    {
                        currentPathNode = 0;
                        currentTransform = enemyTransform;
                        targetPosition = enemyTransform.position;
                    }


                }
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) < 2 && enemyTransform)
        {
            activeState = attackState;
            currentTransform = enemyTransform;
        }


        if (targetPosition == Vector3.zero)
        {
            return;
        }

        if (Vector3.Distance(transform.position, targetPosition) < 2 && enemyTransform == null)
        {
            if (currentPathNode < resultPath.corners.Length-1)
            {
                currentPathNode++;
                fakeTransform.position = resultPath.corners[currentPathNode];
                currentTransform = fakeTransform;
                targetPosition = fakeTransform.position;
            }
            //else
            //{
            //    if (agent.CalculatePath(endPos.position, resultPath))
            //    {
            //        currentPathNode = 1;
            //        targetPosition = resultPath.corners[currentPathNode];
            //        fakeTransform.position = targetPosition;
            //        currentTransform = fakeTransform;
            //    }
            //}
        }

        if (Vector3.Distance(transform.position, targetPosition) > 2)
        {
            activeState = moveState;
        }

        if (Vector3.Distance(transform.position, targetPosition) < 2 && enemyTransform == null)
        {
            activeState = idleState;
        }


        activeState.Execute(currentTransform, Time.deltaTime);

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

    void CheckIfEnemyAlive()
    {
        if(currentTransform.GetComponent<Champion>().dead)
        {
            currentTransform = null;
            targetPosition = Vector3.zero;
            activeState = idleState;
        }
    }

    void Respawn()
    {
        currentTransform = null;
        thisChampion.dead = true;
        GetComponent<Collider>().enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        targetPosition = Vector3.zero;
        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
        Invoke("Spawn", 1f);
    }

    void Spawn()
    {
        transform.position = temporarySpawnSave;

        GetComponent<Collider>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        thisChampion.Init();
        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
        thisChampion.dead = false;
    }
}
