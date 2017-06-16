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
    public bool useRoomBoundary;
    [Range(0, 300)]
    public int spawnDelay = 60;
    [Range(0, 99)]
    public int maxSpawns;
    public bool spawnsOnce = false;
    [Tooltip("Should the spawner start spawning from start?")]
    public bool canSpawn = true;

    private int currentSpawnCount = 0;
    public List<GameObject> spawnedMonsters = new List<GameObject>();
    private Animator spawnerAnimator;
    private Quaternion spawnRotation = Quaternion.identity;
    private PlayerGridObject player;

    private bool coolingDown = true;
    private int cooldownTimer = 0;
    private int randInt;

    //bools for finding where spawner can spawn
    private bool hasChecked = false;
    private bool east = true;
    private bool west = true;
    private bool north = true;
    private bool south = true;

    //colliders to check for where to spawn
    public GameObject eastCollider;
    public GameObject westCollider;
    public GameObject northCollider;
    public GameObject southCollider;

    // Use this for initialization
    protected override void Start()
    {
        spawnerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update() {
        if(coolingDown && currentSpawnCount < maxSpawns)
        {
            cooldownTimer++;

            if (cooldownTimer >= spawnDelay)
            {
                coolingDown = false;
                cooldownTimer = 0;
            }
        }

        if(currentSpawnCount >= maxSpawns)
        {
            ClearDeadSpawns();
        }

        if(canSpawn == true && coolingDown == false && currentSpawnCount < maxSpawns)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        randInt = Random.Range(0, 4);

        if (randInt == 0 && north)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
            currentSpawnCount++;
        }
        else if (randInt == 1 && east)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
            currentSpawnCount++;
        }
        else if (randInt == 2 && west)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            currentSpawnCount++;
        }
        else if (randInt == 3 && south)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
            currentSpawnCount++;
        }

        GameObject summonedMonster = null;
        //null check
        if (enemy)
        {
            summonedMonster = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
            summonedMonster.GetComponent<GenericMonsterBehaviour>().spawner = this;
            spawnedMonsters.Add(summonedMonster);

            PathFindingModule monsterPathFinding = summonedMonster.GetComponentInChildren<PathFindingModule>();
            monsterPathFinding.parameters.tileMap = tileMap;
            monsterPathFinding.parameters.target = targetObj;
            monsterPathFinding.parameters.useRoomBoundary = useRoomBoundary;

            // Activate monster
            summonedMonster.GetComponent<MonsterBehaviourAbstractFSM>().StartAI();
        }

        coolingDown = true;
    }
    
    public void SpawnAtOnce()
    {
        for (int i = 0; i < maxSpawns; i++)
        {
        	SpawnEnemy();
        }
    }

    /// <summary>
    /// Sets up the death flags and allows the animator to take over.
    /// Animator has state machine behaviour scripts to ensure that the entire
    /// death animation will take place.
    /// </summary>
    protected override void Die()
    {
        deathEvent.Invoke();

        hasDied = true;
        isDying = true;

        spawnerAnimator.SetBool("dead", true);
    }

    /// <summary>
    /// Function called by state machine behaviour script to ensure the entire
    /// death animation will take place.
    /// </summary>
    public void InitiateDeathSequence(float deathDuration)
    {
        // Allows the death animation to play fully
        StartCoroutine(DeathSequence(deathDuration)); // Arbitrary death duration
    }

    /// <summary>
    /// Allows the death animation to play fully before spawning items and destroying game object
    /// </summary>
    private IEnumerator DeathSequence(float deathDuration)
    {
        yield return new WaitForSeconds(deathDuration);

        SpawnItem();

        Destroy(this.gameObject);
    }

    /*Code Goal: prevent spawn on terrain
           Bug: causes spawn on start location
    */
    /*
    void OnTriggerStay2D(Collider2D other)
    {
        if (!hasChecked)
        {
            //is touching a terrain object
            if (other.gameObject.GetComponent<TerrainObject>())
            {
                //is touching a barrier
                if (other.gameObject.GetComponent<TerrainObject>().isBarrier)
                {
                    if (other.IsTouching(eastCollider.GetComponent<Collider2D>()))
                    {
                        east = false;
                    }
                    if (other.IsTouching(westCollider.GetComponent<Collider2D>()))
                    {
                        west = false;
                    }
                    if (other.IsTouching(northCollider.GetComponent<Collider2D>()))
                    {
                        north = false;
                    }
                    if (other.IsTouching(southCollider.GetComponent<Collider2D>()))
                    {
                        south = false;
                    }
                }
            }
            
        }
    }
    */
    public int numSpawns() {
    	return currentSpawnCount;
    }

    public void KillSpawns() {
    	int i = 0;

    	while (i < spawnedMonsters.Count) {
    		GameObject obj = spawnedMonsters[i];

    		if (obj == null) {
    			spawnedMonsters.RemoveAt(i);
    		} else {
				KillableGridObject spawn = obj.GetComponent<KillableGridObject>();
                if (spawn.isInvulnerable) spawn.isInvulnerable = false;
    			spawn.TakeScriptedDamage(10000);
				spawnedMonsters.RemoveAt(i);
    		}
    	}

    	currentSpawnCount = 0;
    }

    public void AllowSpawning()
    {
        canSpawn = true;
    }

    public void OnEnable()
    {
    }

    // Disabling is when the active "check mark" in the editor is turned off
    public void OnDisable()
    {
        if (!isDying) KillSpawns();
    }

    private void ClearDeadSpawns()
    {
        // Step through the spawn list backwards so we can remove as we iterate
        for(int i = maxSpawns - 1; i >= 0 ; i--)
        {
            GameObject monster = spawnedMonsters[i];
            if (monster == null)
                spawnedMonsters.RemoveAt(i);
        }

        currentSpawnCount = spawnedMonsters.Count;
    }
}
