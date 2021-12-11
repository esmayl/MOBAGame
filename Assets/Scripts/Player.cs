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
    VisualEffect playerParticles;
    public Vector3 temporarySpawnSave;
    Transform enemy;

    public Champion thisChampion;
    ChampionSkills championSkills;

    NavMeshAgent agent;

    AttackState attackState;
    MovementState moveState;
    IdleState idleState;

    PlayerState activeState;

    LayerMask layerMask;
    Collider[] hits;

    RaycastHit hit;
    bool doingQ = false;
    bool doingW = false;
    bool doingE = false;
    bool doingR = false;


    bool attacking = false;

    void Start()
    {

        InputAction mouseMove = InputHandler.instance.playerInputs.actions.FindAction("Move");
        mouseMove.performed += SetMovePos;

        InputAction skillAction = InputHandler.instance.playerInputs.actions.FindAction("Q");
        skillAction.performed += QCallback;

        skillAction = InputHandler.instance.playerInputs.actions.FindAction("W");
        skillAction.performed += WCallback;

        skillAction = InputHandler.instance.playerInputs.actions.FindAction("E");
        skillAction.performed += ECallback;

        skillAction = InputHandler.instance.playerInputs.actions.FindAction("R");
        skillAction.performed += RCallback;

        playerParticles = GetComponent<VisualEffect>();

        thisChampion = GetComponent<Champion>();
        if (!thisChampion.anim)
        {
            thisChampion.Init();
        }

        thisChampion.championDeath += Respawn;
        thisChampion.levelUp += PlayLevelUpParticles;

        championSkills = GetComponent<ChampionSkills>();

        attackState = new AttackState(gameObject, thisChampion, thisChampion.anim);
        attackState.enemyDead += EnemyDied;
        attackState.endAttack += PlayAttackParticles;

        moveState = new MovementState(gameObject, thisChampion, thisChampion.anim);
        //moveState.beginMove += PlayMovementParticles;

        idleState = new IdleState(gameObject, thisChampion, thisChampion.anim);

        activeState = idleState;

        layerMask = 1 << LayerMask.NameToLayer("Ground");
        layerMask |= 1 << LayerMask.NameToLayer("Attackable");


    }

    void Update()
    {
        if (thisChampion.dead) { return; }

        if (enemy)
        {
            Champion tempChamp = enemy.GetComponent<Champion>();
            if (tempChamp)
            {
                if (tempChamp.dead)
                {
                    enemy = null;
                }
            }
        }

        attackState.counter += Time.deltaTime;

        if ( enemy && !doingQ && !doingW && !doingE && !doingR)
        {
            if (Vector3.Distance(transform.position, enemy.position) < attackRange)
            {
                activeState = attackState;
                attacking = true;
            }
            if (enemy.GetComponent<Champion>().dead)
            {
                activeState = idleState;
                attacking = false;
                enemy = null;
            }
        }
        
        if (!enemy && moveState.ReachedEnd())
        {
            activeState = idleState;
            attacking = false;
        }

        if (doingQ)
        {
            championSkills.Q();
        }
        else if (doingW)
        {
            championSkills.W();
        }
        else if (doingE)
        {
            championSkills.E();
        }
        else if (doingR)
        {
            championSkills.R();
        }
        else if(!thisChampion.dead)
        {
            activeState.Execute(enemy, Time.deltaTime);
        }

    }


    public void SetMovePos(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y,0));

        if (Physics.SphereCast(ray, 0.5f,out hit, 100, layerMask))
        {
            if (Champion.CheckIfEnemy(hit.transform, thisChampion.team))
            {
                enemy = hit.transform;
                moveState.RecalculatePath(enemy.position);
            }
            else
            {
                enemy = null;
                moveState.RecalculatePath(hit.point);
            }
        }

        activeState = moveState;
        thisChampion.anim.SetBool("Moving", true); // Fix to stop moving without animation after skill usage

    }

    public void QCallback(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }
        if (championSkills.QOnCooldown()) { return; }

        Invoke("QOver", championSkills.skills[0].castTime);

        Debug.Log("Doing Q");
        moveState.Stop();

        doingQ = true;
    }

    public void WCallback(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }
        if (championSkills.WOnCooldown()) { return; }

        Invoke("WOver", championSkills.skills[1].castTime);

        Debug.Log("Doing W");

        moveState.Stop();

        doingW = true;
    }

    public void ECallback(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }
        if (championSkills.EOnCooldown()) { return; }

        Invoke("EOver", championSkills.skills[2].castTime);

        Debug.Log("Doing E");

        moveState.Stop();

        doingE = true;
    }

    public void RCallback(InputAction.CallbackContext context)
    {
        if (thisChampion.dead) { return; }
        if (championSkills.ROnCooldown()) { return; }

        Invoke("ROver", championSkills.skills[3].castTime);

        Debug.Log("Doing R");

        moveState.Stop();

        doingR = true;
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

        thisChampion.dead = true;

        GetComponent<VisualEffect>().SendEvent("Die");

        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));

        Invoke("Spawn", 5f);
    }

    void Spawn()
    {
        GetComponent<NavMeshAgent>().Warp(temporarySpawnSave);

        enemy = null;

        GetComponent<Collider>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        thisChampion.Init();
        GetComponent<HealthBar>().UpdateHpBar(1 / ((thisChampion.bi.baseHealth + thisChampion.bi.healthPerLevel * thisChampion.level) / thisChampion.hp));
        thisChampion.dead = false;
    }

    void QOver()
    {
        doingQ = false;
    }

    void WOver()
    {
        doingW = false;
    }

    void EOver()
    {
        doingE = false;
    }

    void ROver()
    {
        doingR = false;
    }
}
