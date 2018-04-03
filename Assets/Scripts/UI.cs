using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public static UI sharedInstance;
    public GameObject towerPrefab;
    public Text resourcePointsText;
    public Text toolPointsText;
    public Button moveBaseButton;
    public Button pathAButton;
    public Button pathBButton;
    public Button guardSpotButton;
    public Button assignSoldiersToTowerButton;

    bool pathButtonsActive;

    [System.NonSerialized]
    public Tower selectedTower;
    
	void Awake () {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pathButtonsActive = false;
    }

    void Start()
    {
        pathAButton.gameObject.SetActive(false);
        pathBButton.gameObject.SetActive(false);
        assignSoldiersToTowerButton.gameObject.SetActive(false);
        UpdateResourcePointsText();
        UpdateToolPointsText();
    }

    public void UpdateResourcePointsText()
    {
        resourcePointsText.text = Resources.sharedInstance.ResourcePoints.ToString() + " Resource Points";
    }

    public void UpdateToolPointsText()
    {
        toolPointsText.text = Resources.sharedInstance.ToolPoints.ToString() + " Tool Points";
    }

    public void BuildTower()
    {
        //Code for creating towers should go here
        if (Resources.sharedInstance.ResourcePoints >= 20)
        {
            Resources.sharedInstance.ResourcePoints -= 20;
        }
        GameObject obj = Instantiate(towerPrefab) as GameObject;
        selectedTower = obj.GetComponent<Tower>();
        assignSoldiersToTowerButton.gameObject.SetActive(true);
    }

    void SetPathButtonsActive(bool active)
    {
        pathButtonsActive = active;
        pathAButton.gameObject.SetActive(pathButtonsActive);
        pathBButton.gameObject.SetActive(pathButtonsActive);
    }

    public void MoveBaseButton()
    {
        SetPathButtonsActive(!pathButtonsActive);
    }

    public void GuardSpotButton()
    {
        //TODO: Assign soldiers to guard a spot
    }

    public void AssignSoldiersToTowerButton()
    {
        //TODO: Assign soldiers to selectedTower
    }

    //0 Path A
    //1 Path B
    public void PathButton(int path)
    {
        SetPathButtonsActive(false);

        //TODO: Move the base along a path
    }
	
	void Update () {
	}
}
