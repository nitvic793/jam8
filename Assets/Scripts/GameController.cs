using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<BaseController> bases;
    public int currentBase = 0;

    void Start()
    {

    }

    void Update()
    {
        MoveSoilder();
        if (Input.GetMouseButtonDown(2)) 
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    var unit = GetOneUnit();
                    unit.guardPosition = hit.point;
                    unit.CurrentState = UnitStates.GUARD;
                }
            }
        }

        if (bases[currentBase].isBaseClear)
        {

        }
    }

    UnitBehavior GetOneUnit()
    {
        var units = FindObjectsOfType<UnitBehavior>();
        foreach(var unit in units)
        {
            if (unit.CurrentState != UnitStates.GUARD)
                return unit;
        }
        return null;
    }

    void MoveSoilder()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform.tag != "Soldier")
            {
                foreach (var soldier in GameObject.FindGameObjectsWithTag("Soldier"))
                {
                    if (soldier.GetComponent<UnitBehavior>().isSelected)
                    {
                        //Move to target location
                        soldier.GetComponent<UnitBehavior>().CurrentState = UnitStates.MOVE;
                        soldier.GetComponent<UnitBehavior>().targetPos = hit.point;
                        //Deselect
                        soldier.GetComponent<UnitBehavior>().isSelected = false;
                    }
                }
            }
        }
    }
}
