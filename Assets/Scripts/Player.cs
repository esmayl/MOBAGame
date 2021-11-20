using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Player : MonoBehaviour
{

    float attackRange = 1;
    VisualEffect playerParticles;
    Vector3 temporarySpawnSave;
    Vector3 targetPosition;
    Transform enemy;

    Champion thisChampion;

    NavMeshAgent agent;

    AttackState attackState;
    MovementState moveState;
    IdleState idleState;

    PlayerState activeState;

    LayerMask layerMask;
    Collider[] hits;

    RaycastHit hit;
    bool doingSkill = false;

    Slider qIcon;


    void Start()
    {
        temporarySpawnSave = transform.position;

        playerParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        thisChampion.championDeath += Respawn;
        thisChampion.levelUp += PlayLevelUpParticles;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = thisChampion.speed * 0.01f;

        attackState = new AttackState(gameObject, thisChampion, thisChampion.anim);
        //attackState.beginAttack += CheckIfEnemyAlive;
        attackState.enemyDead += CheckIfEnemyAlive;
        attackState.endAttack += PlayAttackParticles;

        moveState = new MovementState(gameObject, thisChampion, thisChampion.anim);
        moveState.beginMove += PlayMovementParticles;

        idleState = new IdleState(gameObject, thisChampion, thisChampion.anim);

        qIcon = GameObject.Find("Qicon").GetComponent<Slider>();
        qIcon.value = 1 - (thisChampion.skills[0].cooldown/thisChampion.skills[0].counter);

        activeState = idleState;

        layerMask = 1 << LayerMask.NameToLayer("Attackable");
    }

    void Update()
    {
        if (thisChampion.dead) { return; }

        foreach (SkillState s in thisChampion.skills)
        {
            s.counter += Time.deltaTime;
        }

        attackState.counter += Time.deltaTime;
        doingSkill = false;

        qIcon.value = 1 / (thisChampion.skills[0].cooldown / thisChampion.skills[0].counter);

        //On 1 Click or on holding keyDown player Moves;
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100,~(1 << LayerMask.NameToLayer("Player"))))
            {

                hits = Physics.OverlapSphere(hit.point, 1, layerMask);

                if (hits.Length > 0)
                {


                    enemy = Champion.GetClosestEnemy(transform.position, hits, GetComponent<Collider>(), thisChampion.team);
                    if (enemy)
                    {
                        if (Vector3.Distance(transform.position, enemy.position) > attackRange)
                        {
                            agent.SetDestination(enemy.position);
                            agent.isStopped = false;
                        }

                        targetPosition = enemy.position;
                    }
                }
                else
                {
                    Vector3 temp = hit.point;
                    temp.y = transform.position.y;
                    targetPosition = temp;
                    enemy = null;

                    agent.SetDestination(targetPosition);
                    agent.isStopped = false;
                }
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) < attackRange && enemy && !doingSkill)
        {
            activeState = attackState;
            agent.isStopped = true;
        }


        if (Vector3.Distance(transform.position, targetPosition) > attackRange)
        {
            activeState = moveState;
        }

        if (Vector3.Distance(transform.position, targetPosition) < attackRange && enemy == null)
        {
            agent.isStopped = true;
            activeState = idleState;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            //thisChampion.skills[0].Execute(currentTransform, Time.deltaTime);
            doingSkill = true;
        }
        else
        {
            activeState.Execute(enemy, Time.deltaTime);
        }
    }

    void PlayLevelUpParticles()
    {
        playerParticles.SendEvent("LevelUp");
    }

    void PlayMovementParticles()
    {
        playerParticles.SendEvent("Walk");
    }

    void PlayAttackParticles()
    {
        playerParticles.SetVector3("ParticlePos", enemy.position);
        playerParticles.SendEvent("Play");
    }

    void CheckIfEnemyAlive()
    {
        if (!enemy.GetComponent<Champion>()) { return; }

        if (enemy.GetComponent<Champion>().dead)
        {
            enemy = null;
        }
    }

    void Respawn()
    {
        transform.position = temporarySpawnSave;
        enemy = null;

        GetComponent<VisualEffect>().SendEvent("Die");
        thisChampion.dead = true;
        Invoke("Spawn", 5f);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, targetPosition);
    }
}
