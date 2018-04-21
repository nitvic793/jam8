using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BuilderStates
{
    MOVE,
    BUILD,
    DIE,
    IDLE
};

public enum BuildingType
{
    TOWER,
    WALL
}

public class BuilderBehavior : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMesh;

    float baseDistanceOffset = 5F;
    float relaxDistance = 1.5f;
    public float guardDistanceOffset = 3F;
    public float Health = 100F;
    public float attackVisionDistance = 20F;
    public float attackPoints = 9F;
    public int currentBase = 0;
    public GameObject currentBuildInstance = null;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    public int totalCurrentBuilders = 1;

    public Vector3 buildPosition;
    public Quaternion buildRotation;

    public List<BaseController> bases;

    public BuilderStates CurrentState;
    BuilderStates PreviousState;
    public BuildingType ToBuild;

    public bool isSelected = false;
    bool isIdle = true;
    bool isBuilding = false;
    bool isRunning = false;
    bool isDead = false;


    bool isMoving = false;
    float deathDelay = 0F;

    public Vector3 targetPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        CurrentState = PreviousState = BuilderStates.IDLE;
        targetPos = Vector3.zero;
    }

    void Update()
    {
        navMesh.isStopped = true;
        UpdateLogic();
        switch (CurrentState)
        {
            case BuilderStates.MOVE:
                var isEnemyClose = IsEnemyClose();
                if (Vector3.Distance(transform.position, targetPos) > relaxDistance && !isEnemyClose)
                {
                    isMoving = true;
                    GotoDestination(targetPos);
                }
                else
                {
                    isMoving = false;
                    CurrentState = BuilderStates.IDLE;
                }
                break;
            case BuilderStates.BUILD:
                isEnemyClose = IsEnemyClose();
                isBuilding = false;
                if (Vector3.Distance(transform.position, targetPos) > baseDistanceOffset)
                {
                    isMoving = true;
                    GotoDestination(targetPos);
                }
                else
                {
                    isBuilding = true;
                    isMoving = false;
                }

                Debug.Log("Is building " + isBuilding);
                Debug.Log(currentBuildInstance);
                if (isBuilding && currentBuildInstance == null)
                {
                    var buildPrefab = (ToBuild == BuildingType.WALL) ? wallPrefab : towerPrefab;
                    currentBuildInstance = Instantiate(buildPrefab, buildPosition, buildRotation);
                    var tower = currentBuildInstance.GetComponent<Tower>();
                    tower.delay = tower.delay / totalCurrentBuilders;
                    tower.StartBuilding();
                }
                else if (isBuilding && currentBuildInstance != null)
                {
                    var tower = currentBuildInstance.GetComponent<Tower>();
                    Debug.Log("Tower " + tower.IsReady);
                    if (tower.IsReady)
                    {
                        currentBuildInstance = null;
                        isBuilding = false;
                        CurrentState = BuilderStates.IDLE;
                    }
                }

                Debug.Log(currentBuildInstance);

                break;
            case BuilderStates.IDLE:

                break;
            case BuilderStates.DIE:
                if (deathDelay > 1F)
                {
                    gameObject.SetActive(false);
                }
                deathDelay += Time.deltaTime;
                break;
            default:
                break;
        }

        //UpdateAnimation();
    }

    void UpdateLogic()
    {
        if (Health <= 0)
        {
            isDead = true;
            CurrentState = BuilderStates.DIE;
        }
        if (!isSelected)
        {
            transform.Find("SelectionCirclePrefab").gameObject.SetActive(false);
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
        isBuilding = false;
        isIdle = false;
        isDead = false;
        switch (CurrentState)
        {
            case BuilderStates.MOVE:
                isRunning = isMoving;
                isIdle = !isMoving;
                break;
            case BuilderStates.BUILD:
                isRunning = isMoving;
                isIdle = !isMoving;
                break;
            case BuilderStates.IDLE:
                isBuilding = true;
                break;
            case BuilderStates.DIE:
                isDead = true;
                break;
            default:
                isIdle = true;
                break;
        }

        animator.SetBool("Attack", isBuilding);
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

    bool IsEnemyClose()
    {
        var closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
        {
            return true;
        }

        return false;
    }

    Vector3 GetNextBasePosition()
    {
        var baseStation = bases[currentBase];
        return baseStation.transform.position;
    }

    void GotoDestination(Vector3 position)
    {
        navMesh.isStopped = false;
        navMesh.destination = position;
    }


}
