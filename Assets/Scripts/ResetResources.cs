using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetResources : MonoBehaviour
{
    Resources resourceManager;
    public int resourceCount ;

    public GameObject Soldier;


    private void Start()
    {
        resourceManager = GameObject.Find("HUD_OLD").GetComponent<Resources>();
    }

    public void ResetResourceCount()
    {
        resourceManager.ResourcePoints = 160;
        resourceManager.ToolPoints = 130;
    }

    public Vector3 RandomOffset()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(10.0f, 10.0f));
        return randomPosition;
    }

    public void AddSoldier(Vector3 basePosition, Quaternion rotation)
    {
        for (int i = 0; i < 2; i++)
        {
            var unit = GameObject.Find("Units").GetComponentInChildren<UnitBehavior>();
            var instance = Instantiate(Soldier, (basePosition + RandomOffset()), rotation);
            instance.GetComponent<UnitBehavior>().bases = unit.bases;
            instance.GetComponent<UnitBehavior>().currentBase = unit.currentBase;
            instance.GetComponent<UnitBehavior>().CurrentState = unit.CurrentState;

            // instance.GetComponent<UnitBehavior>(). = unit.bases;

        }
    }
}
