﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour {

    public float delay = 5f;
    public bool IsReady { get; private set; }
    public GameObject fillBarPrefab;
    //FillBar fillBar;

    NavMeshObstacle navMeshObstacle;

    void Awake()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        navMeshObstacle.enabled = false;
        IsReady = false;
        StartCoroutine(Run());
    }
	// Use this for initialization
	void Start () {
        //GameObject obj = Instantiate(fillBarPrefab, transform.position + 7.0f * Vector3.up, Quaternion.identity) as GameObject;
        //fillBar = obj.GetComponent<FillBar>();
	}

    public void StartBuilding()
    {
        
    }


    private void Update()
    {
        
    }

    IEnumerator Run()
    {
        for(float i = 0f; i < delay; i += Time.deltaTime)
        {
            //fillBar.SetScale(i / delay);
            yield return 0;
        }
        IsReady = true;
        navMeshObstacle.enabled = true;
    }
}
