using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public static Resources sharedInstance;
    public int startingResourcePoints;
    public int startingToolPoints;
    private int resourcePoints;
    private int toolPoints;

    public int ResourcePoints
    {
        get
        {
            return resourcePoints;
        }

        set
        {
            resourcePoints = value;
            UI.sharedInstance.UpdateResourcePointsText();
        }
    }
    
    public int ToolPoints
    {
        get
        {
            return toolPoints;
        }

        set
        {
            toolPoints = value;
            UI.sharedInstance.UpdateToolPointsText();
        }
    }

    void Awake()
    {
        resourcePoints = startingResourcePoints;
        toolPoints = startingToolPoints;
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start () {

    }
	
	void Update () {
		
	}
}
