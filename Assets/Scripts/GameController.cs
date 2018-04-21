using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<BaseController> bases;
    Resources resources;
    public int currentBase = 0;

    void Start()
    {
        resources = GameObject.Find("Canvas").GetComponent<Resources>();
    }

    void Update()
    {
        SendBuildCommands();
        MoveSoilder();
        MoveBuilder();    
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

    void SendBuildCommands()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") && resources.ToolPoints >= 20 && resources.ResourcePoints >= 10) // Wall
        {
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    var colliderRotation = hit.collider.transform.rotation;
                    var rotation = new Vector3(-90, 0, 90) + colliderRotation.eulerAngles;
                    rotation.x = -90;
                    foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Builder"))
                    {
                        var builder = selectableObject.GetComponent<BuilderBehavior>();
                        if(builder.isSelected)
                        {
                            //Move to target location
                            builder.GetComponent<BuilderBehavior>().ToBuild = BuildingType.WALL;
                            builder.GetComponent<BuilderBehavior>().CurrentState = BuilderStates.BUILD;
                            builder.GetComponent<BuilderBehavior>().targetPos = hit.point;
                            builder.GetComponent<BuilderBehavior>().buildPosition = hit.point;
                            builder.GetComponent<BuilderBehavior>().buildRotation = Quaternion.Euler(rotation);
                            //Deselect
                            builder.GetComponent<BuilderBehavior>().isSelected = false;
                        }
                    }
                
                    //Instantiate(resources.wallPrefab, hit.point, Quaternion.Euler(rotation));
                    resources.ToolPoints -= 20;
                    resources.ResourcePoints -= 10;
                }
            }
        }

        if (Input.GetButtonDown("Fire2") && resources.ToolPoints >= 10 && resources.ResourcePoints >= 30) // Tower
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    var colliderRotation = hit.collider.transform.rotation;
                    var rotation = colliderRotation.eulerAngles;
                    rotation.x = -90;
                    foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Builder"))
                    {
                        var builder = selectableObject.GetComponent<BuilderBehavior>();
                        if (builder.isSelected)
                        {
                            //Move to target location
                            builder.GetComponent<BuilderBehavior>().ToBuild = BuildingType.TOWER;
                            builder.GetComponent<BuilderBehavior>().CurrentState = BuilderStates.BUILD;
                            builder.GetComponent<BuilderBehavior>().targetPos = hit.point;
                            builder.GetComponent<BuilderBehavior>().buildPosition = hit.point;
                            builder.GetComponent<BuilderBehavior>().buildRotation = Quaternion.Euler(rotation);
                            //Deselect
                            builder.GetComponent<BuilderBehavior>().isSelected = false;
                        }
                    }
                    
                    //Instantiate(resources.towerPrefab, hit.point, Quaternion.Euler(rotation));
                    resources.ToolPoints -= 10;
                    resources.ResourcePoints -= 30;
                }
            }
        }
    }

    UnitBehavior GetOneUnit()
    {
        var units = FindObjectsOfType<UnitBehavior>();
        foreach (var unit in units)
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

    void MoveBuilder()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000F) && hit.transform.tag != "Builder")
            {
                foreach (var builder in GameObject.FindGameObjectsWithTag("Builder"))
                {
                    if (builder.GetComponent<BuilderBehavior>().isSelected)
                    {
                        //Move to target location
                        builder.GetComponent<BuilderBehavior>().CurrentState = BuilderStates.MOVE;
                        builder.GetComponent<BuilderBehavior>().targetPos = hit.point;
                        //Deselect
                        builder.GetComponent<BuilderBehavior>().isSelected = false;
                    }
                }

            }
        }
    }
}
