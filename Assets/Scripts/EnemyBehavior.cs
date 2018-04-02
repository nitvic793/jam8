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

public class EnemyBehavior : MonoBehaviour {

    NavMeshAgent navMesh;

    EnemyStates CurrentState;

	void Start ()
    {
        navMesh = GetComponent<NavMeshAgent>();
        CurrentState = EnemyStates.PURSUE;
	}
	
	void Update ()
    {
		switch(CurrentState)
        {
            case EnemyStates.PURSUE:
                GoTowardsUnit(GetClosestUnitPosition());
                break;
            default:
                break;
        }
	}

    void GoTowardsUnit(Vector3 position)
    {
        navMesh.destination = position;
    }

    Vector3 GetClosestUnitPosition()
    {
        var units = GameObject.FindObjectsOfType<UnitBehavior>();
        var distance = 9000F;
        UnitBehavior closestUnit = null;
        foreach(var unit in units)
        {
            var currentDistance = Vector3.Distance(transform.position, unit.transform.position);
            if(currentDistance<distance)
            {
                closestUnit = unit;
                distance = currentDistance;
            }
        }

        return closestUnit.transform.position;
    }
}
