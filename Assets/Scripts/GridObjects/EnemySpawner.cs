using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : KillableGridObject
{

    public GameObject enemy;
    public TileMap tileMap;
    public GameObject targetObj;
    public UnityEvent deathEvent;
    private Vector3 spawnPosition;

    //Keep track of spawns

    [Header("Spawning Options")]
    public float spawnDelay;
    public int maxSpawns;
    public bool spawnsOnce = false;
    [Tooltip("Should the spawner start spawning from start?")]
    public bool spawnOnStart = true;

    private int currSpawns = 0;
    private List<GameObject> list = new List<GameObject>();
    private Animator animator;
    private Quaternion spawnRotation = Quaternion.identity;
    private PlayerGridObject player;
    private Coroutine spawningCoroutine = null;

    System.Random randGen = new System.Random();
    private int randInt;

    // Use this for initialization
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGridObject>();
        animator = GetComponent<Animator>();
        if(spawnOnStart)
        {
            if (spawnsOnce)
                SpawnRandomDir();
            else
                spawningCoroutine = StartCoroutine(SpawnRandomDir());
        }
    }

    // Update is called once per frame
    protected override void Update() {
        if (health <= 0)
        {
            StartCoroutine(waitForDeathAnim());
        }
        
        foreach(GameObject obj in list)
        {
            if (obj == null)
            {
                currSpawns--;
                list.Remove(obj);
                break; //prevents error from modifying list during foreach loop
            }
        }
    

    }

    void SpawnEnemy()
    {
		randInt = randGen.Next(0, 4);

        if (randInt == 1)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
        }
        else if (randInt == 2)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
        }
        else if (randInt == 3)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
        }
        else
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
        }
        GameObject enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
        list.Add(enemyObj);

        GenericMonsterBehaviour monsterBehaviour = enemyObj.GetComponent<GenericMonsterBehaviour>();
        monsterBehaviour.pathFindingParameters.tileMap = tileMap;
        monsterBehaviour.pathFindingParameters.target = targetObj;

        currSpawns++;
    }

    public void SpawnAtOnce()
    {
        for (int i = 0; i < maxSpawns; i++)
        {
        	SpawnEnemy();
        }
    }

    IEnumerator SpawnRandomDir()
    {
        while (health > 0)
        {
            if (currSpawns < maxSpawns)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void BeginSpawning()
    {
        if (spawningCoroutine != null)
            spawningCoroutine = StartCoroutine(SpawnRandomDir());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && player.isAttacking)
        {
            TakeDamage(player.damage);
        }
    }


    /* deleting this function as the killablegridobject function does the same thing
    public override bool TakeDamage(int dmg)
    {
        gameObject.GetComponent<Animation>().Play("Damaged");
        return base.TakeDamage(dmg);
    }
    */

    protected override void Die() {
		deathEvent.Invoke();
        base.Die();
    }

    IEnumerator waitForDeathAnim()
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(1.2f);
        this.gameObject.SetActive(false);
    }
    public int numSpawns() {
    	return currSpawns;
    }

    public void KillSpawns() {
    	int i = 0;

    	while (i < list.Count) {
    		GameObject obj = list[i];

    		if (obj == null) {
    			list.RemoveAt(i);
    		} else {
				KillableGridObject spawn = obj.GetComponent<KillableGridObject>();
    			spawn.TakeDamage(10000);
				list.RemoveAt(i);
    		}
    	}

    	currSpawns = 0;
    }
}
