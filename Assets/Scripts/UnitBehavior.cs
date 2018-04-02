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

public class UnitBehavior : MonoBehaviour {

    NavMeshAgent navMesh;

    UnitStates CurrentState;

	void Start () {
        navMesh = GetComponent<NavMeshAgent>();
        CurrentState = UnitStates.GO_TO_NEXT_BASE;
	}
	
	void Update () {
        switch(CurrentState)
        {
            case UnitStates.GO_TO_NEXT_BASE:
                GotoDestination(GetNextBasePosition());
                break;
            default:
                break;
        }
	}

    Vector3 GetNextBasePosition()
    {
        return GameObject.Find("Cube").transform.position;
    }

    void GotoDestination(Vector3 position)
    {
        navMesh.destination = position;
    }
}
