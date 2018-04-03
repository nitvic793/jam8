using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    public float attackVisionDistance = 20F;
    public float attackPoints = 20F;
    float attackTime = 0F;
    float attackPeriod = 0.5F;
    public GameObject muzzleFlash;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(attackTime>attackPeriod)
        {
            attackTime = 0F;
            var enemy = GetClosestEnemy();
            if(enemy!=null)
            {
                var pos = transform.position + transform.forward * 8;
                var flash = Instantiate(muzzleFlash, pos, Quaternion.LookRotation(transform.forward));
                flash.transform.localScale = new Vector3(10, 10, 10);
                enemy.InflictDamage(attackPoints);//Attack
            }
        }

        attackTime += Time.deltaTime;
	}

    EnemyBehavior GetClosestEnemy()
    {
        var distance = 9000F;
        var enemies = GameObject.FindObjectsOfType<EnemyBehavior>();
        EnemyBehavior closestEnemy = null;
        foreach (var enemy in enemies)
        {
            var currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < attackVisionDistance && currentDistance < distance)
            {
                distance = currentDistance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
