﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public static Resources sharedInstance;
    public int startingResourcePoints;
    public int startingToolPoints;
    private int resourcePoints;
    private int toolPoints;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    RaycastHit hit;
    Ray ray;

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
	
	void Update ()
    {
        if (Input.GetButtonDown("Fire1") && toolPoints >= 20 && resourcePoints >= 10) // Wall
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    Instantiate(wallPrefab, hit.point, Quaternion.identity);
                    toolPoints -= 20;
                    resourcePoints -= 10;
                }
            }
        }
        if (Input.GetButtonDown("Fire2") && toolPoints >= 10 && resourcePoints >= 30) // Tower
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    Instantiate(towerPrefab, hit.point, Quaternion.identity);
                    toolPoints -= 10;
                    resourcePoints -= 30;
                }
            }
        }

    }
}
