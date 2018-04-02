using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyStates
{
    PURSUE,
    ATTACK,
    DIE
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
    bool isDead = false;

    float attackCycleTime = 0F;

    public float Health = 100F;
    public float attackDistance = 1F;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        CurrentState = PreviousState = EnemyStates.PURSUE;
    }

    void Update()
    {
        UpdateAnimation();
        navMesh.isStopped = true;
        switch (CurrentState)
        {
            case EnemyStates.PURSUE:
                var pos = GetClosestUnitPosition();
                if (Vector3.Distance(transform.position, pos) < attackDistance)
                {
                    CurrentState = EnemyStates.ATTACK;
                }
                else
                {
                    GoTowardsUnit(pos);
                }
                break;
            case EnemyStates.ATTACK:
             
                var unit = GetClosestUnit();
                if (Vector3.Distance(transform.position, unit.transform.position) > attackDistance)
                {
                    CurrentState = EnemyStates.PURSUE;
                }
                attackCycleTime += Time.deltaTime;
                if (attackCycleTime > 0.4F)
                {
                    attackCycleTime = 0F;
                    //Attack
                }
                break;
            case EnemyStates.DIE:

            default:
                break;
        }
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
            default:
                isIdle = true;
                break;
        }

        animator.SetBool("Attack", isAttacking);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Die", isDead);
        animator.SetBool("Idle", isIdle);
    }

    UnitBehavior GetClosestUnit()
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
