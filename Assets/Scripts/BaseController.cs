using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    ResetResources resourceManager;

    public List<Transform> spawnPoints;
    public GameObject monsterPrefab;
    public bool isBaseClear = false;
    public int totalWaves = 2;
    public float spawnTime = 4;
    public int spawnCount = 5;
    public bool isCurrentBase = false;


    float currentTime = 0F;
    int currentWave = 0;

    List<GameObject> monsters;
    SpriteRenderer miniMapIcon;
    // Use this for initialization
    void Start()
    {
        resourceManager = GameObject.Find("Canvas").GetComponent<ResetResources>();
        miniMapIcon = gameObject.GetComponentInChildren<SpriteRenderer>();
        monsters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCurrentBase)
        {
            if (currentTime >= spawnTime && !isBaseClear)
            {
                currentTime = 0F;
                SpawnMonsters();
                currentWave++;
            }

            if (currentWave >= totalWaves)
            {
                isBaseClear = true;
                foreach (var monster in monsters)
                {
                    if (!monster.GetComponent<EnemyBehavior>().isDead)
                        isBaseClear = false;
                }
            }

            currentTime += Time.deltaTime;
        }
        if (isBaseClear)
        {
            miniMapIcon.color = Color.green;
        }
    }
   
    public void setAsCurrentBase()
    {
        if (isCurrentBase == false)
        {
            resourceManager.ResetResourceCount();
            resourceManager.AddSoldier(transform.position, transform.rotation);
        }
        isCurrentBase = true;
    }

    void SpawnMonsters()
    {
        if (spawnPoints.Count == 0) return;
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        var spawnPoint = spawnPoints[spawnPointIndex];
        for (var i = 0; i < spawnCount; ++i)
        {
            var pos = spawnPoint.transform.position;
            pos.x += Random.Range(1F, 10F);
            pos.z += Random.Range(1F, 10F);
            var monster = Instantiate(monsterPrefab, pos, spawnPoint.transform.rotation);
            monsters.Add(monster);
        }
    }
}
