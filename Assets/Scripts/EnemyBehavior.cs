using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyStates
{
    PURSUE,
    ATTACK,
    DIE,
    IDLE
}

public class EnemyBehavior : MonoBehaviour
{

    NavMeshAgent navMesh;
    Animator animator;

    EnemyStates CurrentState;
    EnemyStates PreviousState;

    bool isRunning = false;
    bool isAttacking = false;
    bool isIdle = true;
    public bool isDead = false;

    float deathDelay = 0F;
    float attackCycleTime = 0F;

    public float Health = 100F;
    public float attackDistance = 10F;
    public float attackPoints = 12F;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        CurrentState = PreviousState = EnemyStates.PURSUE;
    }

    void Update()
    {
        UpdateLogic();
        navMesh.isStopped = true;
        switch (CurrentState)
        {
            case EnemyStates.PURSUE:
                var closeUnit = GetClosestUnit();
                if(closeUnit==null)
                {
                    CurrentState = EnemyStates.IDLE;
                    break;
                }

                var pos = closeUnit.transform.position;
                if (Vector3.Distance(transform.position, pos) < attackDistance)
                {
                    PreviousState = CurrentState;
                    CurrentState = EnemyStates.ATTACK;
                }
                else
                {
                    GoTowardsUnit(pos);
                }

                break;
            case EnemyStates.ATTACK:             
                var unit = GetClosestUnit();
                if(unit==null)
                {
                    CurrentState = PreviousState;
                    break;
                }

                if (Vector3.Distance(transform.position, unit.transform.position) > attackDistance)
                {
                    CurrentState = EnemyStates.PURSUE;
                }
                attackCycleTime += Time.deltaTime;
                if (attackCycleTime > 0.8F)
                {
                    attackCycleTime = 0F;
                    unit.InflictDamage(attackPoints);//Attack
                }

                break;
            case EnemyStates.DIE:
                if(deathDelay>1F)
                {
                    gameObject.SetActive(false);
                }

                deathDelay += Time.deltaTime;
                break;
            default:
                break;
        }

        UpdateAnimation();
    }

    void UpdateLogic()
    {
        if (Health<=0)
        {
            isDead = true;
            CurrentState = EnemyStates.DIE;
        }
    }

    public void InflictDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0) Health = 0F;
    }

    void UpdateAnimation()
    {
        isRunning = false;
        isAttacking = false;
        isIdle = false;
        isDead = false;
        switch (CurrentState)
        {
            case EnemyStates.PURSUE:
                isRunning = true;
                break;
            case EnemyStates.ATTACK:
                isAttacking = true;
                break;
            case EnemyStates.DIE:
                isDead = true;
                break;
            case EnemyStates.IDLE:
            default:
                isIdle = true;
                break;
        }

        animator.SetBool("Attack", isAttacking);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Die", isDead);
        animator.SetBool("Idle", isIdle);
    }

    BaseUnit GetClosestUnit()
    {
        var units = GameObject.FindObjectsOfType<BaseUnit>();
        var distance = 9000F;
        BaseUnit closestUnit = null;
        foreach (var unit in units)
        {
            var currentDistance = Vector3.Distance(transform.position, unit.transform.position);
            if (currentDistance < distance)
            {
                closestUnit = unit;
                distance = currentDistance;
            }
        }

        return closestUnit;
    }

    void GoTowardsUnit(Vector3 position)
    {
        navMesh.isStopped = false;
        navMesh.destination = position;
    }

    Vector3 GetClosestUnitPosition()
    {
        var units = GameObject.FindObjectsOfType<UnitBehavior>();
        var distance = 9000F;
        UnitBehavior closestUnit = null;
        foreach (var unit in units)
        {
            var currentDistance = Vector3.Distance(transform.position, unit.transform.position);
            if (currentDistance < distance)
            {
                closestUnit = unit;
                distance = currentDistance;
            }
        }

        return closestUnit.transform.position;
    }
}
