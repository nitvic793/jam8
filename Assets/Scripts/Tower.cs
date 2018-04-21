using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    public float delay = 5f;
    public float currentTime = 0F;
    public bool IsReady { get; private set; }
    public GameObject fillBarPrefab;
    //FillBar fillBar;
    NavMeshObstacle navMeshObstacle;
    public Canvas canvas;

    void Awake()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        navMeshObstacle.enabled = false;
        IsReady = false;
        StartCoroutine(Run());
    }
    // Use this for initialization
    void Start()
    {
    }

    public void StartBuilding()
    {

    }

    void Update()
    {
        var fillbar = fillBarPrefab.GetComponent<RectTransform>();
        if (currentTime >= delay)
            canvas.enabled = false;
        else
            fillbar.sizeDelta = new Vector2((currentTime / delay) * 10, 1);
    }

    IEnumerator Run()
    {
        for (float i = 0f; i < delay; i += Time.deltaTime)
        {
            currentTime = i;
            yield return 0;
        }
        currentTime = delay + 0.01F;
        IsReady = true;
        navMeshObstacle.enabled = true;
    }
}
