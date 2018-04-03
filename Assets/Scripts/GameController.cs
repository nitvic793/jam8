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
}
