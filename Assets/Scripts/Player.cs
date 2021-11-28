using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


[RequireComponent(typeof(Champion),typeof(HealthBar))]
public class Player : MonoBehaviour
{

    float attackRange = 3;
    float stopDistance = 0.5f;
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

    PlayerInput input;

    public Skill[] skillPrefabs;
    public SkillState[] skills;
    GameObject[] skillInstances;


    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.ActivateInput();

        InputAction mouseMove = input.actions.FindAction("Move");
        mouseMove.performed += SetMovePos;

        InputAction qAction = input.actions.FindAction("Q");
        qAction.performed += DoQ;

        temporarySpawnSave = transform.position;

        playerParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        thisChampion.Init();

        thisChampion.championDeath += Respawn;
        thisChampion.levelUp += PlayLevelUpParticles;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = thisChampion.speed * 0.01f;

        attackState = new AttackState(gameObject, thisChampion, thisChampion.anim);
        attackState.enemyDead += EnemyDied;
        attackState.endAttack += PlayAttackParticles;

        moveState = new MovementState(gameObject, thisChampion, thisChampion.anim);
        //moveState.beginMove += PlayMovementParticles;

        idleState = new IdleState(gameObject, thisChampion, thisChampion.anim);

        qIcon = GameObject.Find("Qicon").GetComponent<Slider>();
        //qIcon.value = 1 - (thisChampion.skills[0].cooldown/thisChampion.skills[0].counter);

        activeState = idleState;

        layerMask = 1 << LayerMask.NameToLayer("Attackable");



        //if(skillPrefabs.Length <= 0)
        //{
        //	return;
        //}

        //skills = new SkillState[4];

        ////Instantiate all skillPrefabs
        //skillInstances = new GameObject[skillPrefabs.Length];
        //int i = 0;

        //foreach (Skill g in skillPrefabs)
        //{
        //	skillInstances[i] = Instantiate(g.skillPrefab);
        //	i++;
        //}

        //skills[0] = new AhriQ(gameObject, this, anim, skillInstances[0]);
        //skills[1] = new AhriW(gameObject, this, anim, skillInstances[1]);
        //skills[2] = new AhriE(gameObject, this, anim, skillInstances[2]);
        //skills[3] = new AhriR(gameObject, this, anim, skillInstances[3]);
    }

    void Update()
    {
        if (thisChampion.dead) { return; }

        if (enemy)
        {
            if (enemy.GetComponent<Champion>().dead)
            {
                enemy = null;
            }
        }

        //foreach (SkillState s in thisChampion.skills)
        //{
        //    s.counter += Time.deltaTime;
        //}

        attackState.counter += Time.deltaTime;
        doingSkill = false;

        //qIcon.value = 1 / (thisChampion.skills[0].cooldown / thisChampion.skills[0].counter);

        if (doingSkill)
        {
            skills[0].Execute(null, Time.deltaTime);
            doingSkill = false;
        }
        else
        {
            activeState.Execute(enemy, Time.deltaTime);
        }

        if (targetPosition == Vector3.zero)
        {
            return;
        }


        if (Vector3.Distance(transform.position, targetPosition) < attackRange && enemy && !doingSkill)
        {
            activeState = attackState;
            agent.isStopped = true;
        }
        if (Vector3.Distance(transform.position, targetPosition) > attackRange && enemy)
        {
            activeState = moveState;
        }

        if(Vector3.Distance(transform.position, targetPosition) > stopDistance && enemy == null)
        {
            activeState = moveState;
            agent.isStopped = false;
        }

        if (Vector3.Distance(transform.position, targetPosition) < stopDistance && enemy == null)
        {
            agent.isStopped = true;
            activeState = idleState;
        }


        activeState.Execute(enemy, Time.deltaTime);

        agent.SetDestination(targetPosition);

    }


    public void SetMovePos(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y,0));

        if (Physics.SphereCast(ray, 0.5f,out hit, 100, ~(1 << LayerMask.NameToLayer("Player"))))
        {

            agent.isStopped = true;

            enemy = hit.transform;

            if (Champion.CheckIfEnemy(enemy,thisChampion.team))
            {
                if (Vector3.Distance(transform.position, enemy.position) > attackRange)
                {
                    activeState = moveState;

                    targetPosition = enemy.position;

                    agent.isStopped = false;
                }
            }
            else
            {
                Vector3 temp = hit.point;
                temp.y = transform.position.y;
                targetPosition = temp;
                enemy = null;

                agent.isStopped = false;
            }
        }

    }

    public void DoQ(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }

        Debug.Log("Doing Q");

        doingSkill = true;
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

    void EnemyDied()
    {
        enemy = null;
    }

    void Respawn()
    {
        if (thisChampion.dead) { return; }

        transform.position = temporarySpawnSave;
        enemy = null;

        GetComponent<VisualEffect>().SendEvent("Die");
        thisChampion.dead = true;
        targetPosition = Vector3.zero;

        Invoke("Spawn", 5f);
    }

    void Spawn()
    {
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
