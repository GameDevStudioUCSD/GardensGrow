using System;
using UnityEngine;
using UnityEngine.Events;


public class EnemySpawnerScript : KillableGridObject
{
    public GameObject enemy;
    public TileMap tileMap;
    public GameObject playerObj;
    public UnityEvent deathEvent;

    System.Random randGen = new System.Random();
    private int randInt;
    public float spawnDelay;
    private float elapsedTime;

    // Use this for initialization
    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime++ > spawnDelay)
        {

            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
            spawnRandomDir();

            elapsedTime = 0;
        }

    }
    void spawnRandomDir()
    {
        randInt = randGen.Next(0, 4);

        GameObject enemyObj;
        
        if(randInt == 1)
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

    }
    protected override void Die() {
        base.Die();
        deathEvent.Invoke();
    }
}
