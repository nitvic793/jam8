using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitStates
{
    MOVE,
    GO_TO_NEXT_BASE,
    GUARD,
    ATTACK,
    DIE,
    IDLE
};

public class UnitBehavior : BaseUnit
{
    Animator animator;
    NavMeshAgent navMesh;

    public GameObject muzzleFlash;
    float baseDistanceOffset = 15F;
    float relaxDistance = 1.5f;
    public float guardDistanceOffset = 3F;
    public float Health = 100F;
    public float attackVisionDistance = 20F;
    public float attackPoints = 9F;
    public int currentBase = 0;
    public Vector3 guardPosition;
    public GameObject nextBase;
    public GameObject pathToBase;

    public List<BaseController> bases;

    public UnitStates CurrentState;
    UnitStates PreviousState;

    public bool isSelected = false;
    bool isIdle = true;
    public bool isAttacking = false;
    bool isRunning = false;
    bool isDead = false;

    bool isMoving = false;
    float attackCycleTime = 0F;
    float deathDelay = 0F;

    public AudioClip shoot1;

    

    public Vector3 targetPos;

    
    void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        CurrentState = PreviousState = UnitStates.GO_TO_NEXT_BASE;
        targetPos = Vector3.zero;
    }

    void Update()
    {
        navMesh.isStopped = true;
        UpdateLogic();
        switch (CurrentState)
        {
            case UnitStates.MOVE:
                var isEnemyClose = AttackEnemyIfClose();
                if (Vector3.Distance(transform.position, targetPos) > relaxDistance && !isEnemyClose)
                {
                 
                    isMoving = true;
                    GotoDestination(targetPos);
                }
                else
                {
                    isMoving = false;
                }
                break;
            case UnitStates.GO_TO_NEXT_BASE:
                var nextBasePos = GetNextBasePosition();
                isEnemyClose = AttackEnemyIfClose();
               
                if (Vector3.Distance(transform.position, nextBasePos) > baseDistanceOffset && !isEnemyClose)
                {
                    isMoving = true;
                    GotoDestination(nextBasePos);
                }
                else
                {
                    isMoving = false;
                    if(!isEnemyClose)
                    {
                        bases[currentBase].setAsCurrentBase();
                    }
                }
                break;
            case UnitStates.GUARD:
                var guardPos = GetClosestGuardPosition();
                isEnemyClose = AttackEnemyIfClose();
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


                // execute block of code here
                AudioManager.instance.RandomizeShootSfx(shoot1);


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
        if(!isSelected)
        {
            transform.Find("SelectionCirclePrefab").gameObject.SetActive(false);
        }
    }

    public override void InflictDamage(float amount)
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
            case UnitStates.MOVE:
                isRunning = isMoving;
                isIdle = !isMoving;
                break;
            case UnitStates.GO_TO_NEXT_BASE:
                isRunning = isMoving;
                isIdle = !isMoving;
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
        var baseStation = bases[currentBase];
        return baseStation.transform.position;
    }

    Vector3 GetClosestGuardPosition()
    {
        return guardPosition;
    }

    void GotoDestination(Vector3 position)
    {
        navMesh.isStopped = false;
        navMesh.destination = position;
    }

   
}
