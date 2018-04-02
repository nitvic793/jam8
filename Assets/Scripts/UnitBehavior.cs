using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum UnitStates
{
    GO_TO_NEXT_BASE,
    GUARD,
    ATTACK,
    DIE
};

public class UnitBehavior : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMesh;

    public GameObject muzzleFlash;
    public float baseDistanceOffset = 3F;
    public float Health = 100F;
    public float attackVisionDistance = 20F;
    public float attackPoints = 9F;

    UnitStates CurrentState;
    UnitStates PreviousState;

    bool isIdle = true;
    bool isAttacking = false;
    bool isRunning = false;
    bool isDead = false;

    bool isMoving = false;
    float attackCycleTime = 0F;
    float deathDelay = 0F;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        CurrentState = PreviousState = UnitStates.GUARD;
    }

    void Update()
    {
        UpdateLogic();
        navMesh.isStopped = true;
        switch (CurrentState)
        {
            case UnitStates.GO_TO_NEXT_BASE:
                GotoDestination(GetNextBasePosition());
                AttackEnemyIfClose();
                break;
            case UnitStates.GUARD:
                var guardPos = GetClosestGuardPosition();
                var isEnemyClose = AttackEnemyIfClose();
                if (Vector3.Distance(transform.position, guardPos) > baseDistanceOffset && !isEnemyClose)
                {
                    isMoving = true;
                    GotoDestination(guardPos);
                }
                else
                {
                    isMoving = false;
                }
                break;
            case UnitStates.ATTACK:
                attackCycleTime += Time.deltaTime;
                var enemy = GetClosestEnemy();
                if (enemy == null)
                {
                    CurrentState = PreviousState;
                    break;
                }

                transform.LookAt(enemy.transform);
                if (attackCycleTime > 0.8F)
                {
                    attackCycleTime = 0F;
                    var pos = transform.position;
                    pos.y += 2;
                    pos = pos + transform.forward * 1;
                    
                    var flash = Instantiate(muzzleFlash, pos, Quaternion.LookRotation(transform.forward));
                    enemy.InflictDamage(attackPoints);//Attack
                }
                break;
            case UnitStates.DIE:
                if (deathDelay > 1F)
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
        if (Health <= 0)
        {
            isDead = true;
            CurrentState = UnitStates.DIE;
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
            case UnitStates.GO_TO_NEXT_BASE:
                isRunning = true;
                break;
            case UnitStates.ATTACK:
                isAttacking = true;
                break;
            case UnitStates.DIE:
                isDead = true;
                break;
            case UnitStates.GUARD:
                isRunning = isMoving;
                isIdle = !isMoving;
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

    EnemyBehavior GetClosestEnemy()
    {
        var distance = 9000F;
        var enemies = GameObject.FindObjectsOfType<EnemyBehavior>();
        EnemyBehavior closestEnemy = null;
        foreach (var enemy in enemies)
        {
            var currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < attackVisionDistance && currentDistance < distance)
            {
                distance = currentDistance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    bool AttackEnemyIfClose()
    {
        var closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            PreviousState = CurrentState;
            CurrentState = UnitStates.ATTACK;
            return true;
        }

        return false;
    }

    Vector3 GetNextBasePosition()
    {
        return GameObject.Find("Cube").transform.position;
    }

    Vector3 GetClosestGuardPosition()
    {
        return GameObject.Find("BaseMiddle").transform.position;
    }

    void GotoDestination(Vector3 position)
    {
        navMesh.isStopped = false;
        navMesh.destination = position;
    }
}
