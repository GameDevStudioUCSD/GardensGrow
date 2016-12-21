﻿using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnerScript : KillableGridObject
{
    public GameObject enemy;
    public TileMap tileMap;
    public GameObject playerObj;
    public UnityEvent deathEvent;
    public float spawnDelay;

    private PlayerGridObject player;

    System.Random randGen = new System.Random();
    private int randInt;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGridObject>();
        StartCoroutine(spawnRandomDir());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
            this.Die();
        }

    

    }

    IEnumerator spawnRandomDir()
    {
        while (health > 0)
        {
            randInt = randGen.Next(0, 4);

            GameObject enemyObj;

            if (randInt == 1)
            {
                Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
                Quaternion spawnRotation = Quaternion.identity;
                enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
            }
            else if (randInt == 2)
            {
                Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
                Quaternion spawnRotation = Quaternion.identity;
                enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
            }
            else if (randInt == 3)
            {
                Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
                Quaternion spawnRotation = Quaternion.identity;
                enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
                Quaternion spawnRotation = Quaternion.identity;
                enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
            }

            // TODO: change this later to fit other AI
            // Give AI info about the tile map and the target object
            enemyObj.GetComponent<SmartChasingMonster>().tileMap = tileMap;
            enemyObj.GetComponent<SmartChasingMonster>().targetObject = playerObj;

            yield return new WaitForSeconds(spawnDelay);
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && player.isAttacking)
        {
            TakeDamage(player.damage);
        }
    }
    public override bool TakeDamage(int dmg)
    {
        gameObject.GetComponent<Animation>().Play("Damaged");
        return base.TakeDamage(dmg);
    }
    protected override void Die() {
        base.Die();
        deathEvent.Invoke();
    }
}
