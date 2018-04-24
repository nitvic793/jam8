using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public static UI sharedInstance;
    public GameObject towerPrefab;
    public Text resourcePointsText;
    public Text toolPointsText;
    public Text soldiersText;
    public Text buildersText;
    public Button moveBaseButton;
    public Button pathAButton;
    public Button pathBButton;
    public Button guardSpotButton;
    public Button assignSoldiersToTowerButton;
    public Image winScreen;
    public Image loseScreen;
    private bool gameEnded; //has player won or lost
    private GameObject[] bases; //bases to check if player has won yet

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
        UpdateSoldiersText();
        UpdateBuildersText();
        gameEnded = false; //has player won or lost yet?
        bases = GameObject.FindGameObjectsWithTag("Base");
    }

    private void OnGUI()
    {
        UpdateResourcePointsText();
        UpdateToolPointsText();
        UpdateSoldiersText();
        UpdateBuildersText();

        //if out of soldiers and builders, lose
        if((GameObject.FindGameObjectsWithTag("Soldier").Length <= 0 || GameObject.FindGameObjectsWithTag("Builder").Length <= 0) && gameEnded == false)
        {
            LoseGame();
            gameEnded = true;
        }

        //if all bases are clear, win
        foreach(GameObject g in bases)
        {
            if(g.GetComponent<BaseController>().isBaseClear == false)
            {
                break;
            }
            WinGame();
            gameEnded = true;
        }
    }

    public void UpdateResourcePointsText()
    {
        resourcePointsText.text = Resources.sharedInstance.ResourcePoints.ToString() + " Resource Points";
    }

    public void UpdateToolPointsText()
    {
        toolPointsText.text = Resources.sharedInstance.ToolPoints.ToString() + " Tool Points";
    }

    public void UpdateSoldiersText()
    {
        soldiersText.text = GameObject.FindGameObjectsWithTag("Soldier").Length + " Soldiers";
    }

    public void UpdateBuildersText()
    {
        buildersText.text = GameObject.FindGameObjectsWithTag("Builder").Length + " Builders";
    }

    public void WinGame()
    {
        winScreen.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        loseScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
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
        var u = FindObjectOfType<UnitBehavior>();
        var bases = u.bases;
        var b = bases[u.currentBase];

        //SetPathButtonsActive(!pathButtonsActive);
        if(b.isBaseClear)
        {
            var units = FindObjectsOfType<UnitBehavior>();
            foreach (var unit in units)
            {
                unit.currentBase++;
                unit.CurrentState = UnitStates.GO_TO_NEXT_BASE;
            }
        }     
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
