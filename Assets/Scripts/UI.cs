using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public static UI sharedInstance;
    public Text resourcePointsText;
    public Text toolPointsText;
    
	void Awake () {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    void Start()
    {
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
        if (Resources.sharedInstance.ResourcePoints >= 20)
        {
            Resources.sharedInstance.ResourcePoints -= 20;
        }
    }
	
	void Update () {
		
	}
}
