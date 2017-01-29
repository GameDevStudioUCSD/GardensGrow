using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindSlime : PathFindingMonster {

    public GameObject littleSlime;

    public float spawnDelay;
    private Vector3 spawnPosition;
    private List<GameObject> list = new List<GameObject>();
    private Quaternion spawnRotation = Quaternion.identity;
    System.Random randGen = new System.Random();
    private int randInt;
    private int currentSpawn = 0;
    public int maxSpawn;
    private GameObject enemyObj;
    
    void Start()
    {
        //StartCoroutine(SpawnRandomDir());
    }
    
    void SpawnEnemy()
    {
		randInt = randGen.Next(0, 4);

        if (randInt == 1)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
            enemyObj = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
            list.Add(enemyObj);
            enemyObj.GetComponent<PathFindingMonster>().tileMap = tileMap;
            enemyObj.GetComponent<PathFindingMonster>().targetObject = targetObject;
        }
        else if (randInt == 2)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
            enemyObj = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
            list.Add(enemyObj);
            enemyObj.GetComponent<PathFindingMonster>().tileMap = tileMap;
            enemyObj.GetComponent<PathFindingMonster>().targetObject = targetObject;
        }
        else if (randInt == 3)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            enemyObj = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
            list.Add(enemyObj);
            enemyObj.GetComponent<PathFindingMonster>().tileMap = tileMap;
            enemyObj.GetComponent<PathFindingMonster>().targetObject = targetObject;
        }
        else
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
            enemyObj = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
            list.Add(enemyObj);
            enemyObj.GetComponent<PathFindingMonster>().tileMap = tileMap;
            enemyObj.GetComponent<PathFindingMonster>().targetObject = targetObject;
        }
        //update lil'slime's tilemap and targetobject*
        enemyObj.GetComponent<littleWindSlime>().tileMap = this.gameObject.GetComponent<WindSlime>().tileMap;
        enemyObj.GetComponent<littleWindSlime>().targetObject = this.gameObject.GetComponent<WindSlime>().targetObject;


        if (currentSpawn < maxSpawn)    {

            currentSpawn++;
        }
    }

    IEnumerator SpawnRandomDir()
    {
        while (health > 0)
        {
            if(currentSpawn < maxSpawn)    {

                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return 0;
        }
        yield return 0;
    }
}
