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
    [Range(0.0f, 60.0f)]
    public float spawnDelay = 3.0f;
    [Range(0, 99)]
    public int maxSpawns;
    public bool spawnsOnce = false;
    [Tooltip("Should the spawner start spawning from start?")]
    public bool canSpawn = true;

    private int currSpawns = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private Animator spawnerAnimator;
    private Quaternion spawnRotation = Quaternion.identity;
    private PlayerGridObject player;
    private Coroutine spawningCoroutine = null;

    private bool coolingDown = true;
    private float cooldownTimer = 0.0f;
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

    // Used for initialization
    private bool wasInitialized = false;
    
    
    // Use this for initialization
    protected override void Start()
    {
        spawnerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update() {
        if(coolingDown == true)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > spawnDelay)
            {
                coolingDown = false;
                cooldownTimer = 0.0f;
            }
        }

        if(currSpawns >= maxSpawns)
        {
            ClearDeadSpawns();
        }

        if(canSpawn == true && coolingDown == false && currSpawns < maxSpawns)
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
            currSpawns++;
        }
        else if (randInt == 1 && east)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
            currSpawns++;
        }
        else if (randInt == 2 && west)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            currSpawns++;
        }
        else if (randInt == 3 && south)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
            currSpawns++;
        }
        GameObject summonedMonster = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
        spawnedMonsters.Add(summonedMonster);

        PathFindingModule monsterPathFinding = summonedMonster.GetComponentInChildren<PathFindingModule>();
        monsterPathFinding.parameters.tileMap = tileMap;
        monsterPathFinding.parameters.target = targetObj;

        // Activate monster
        summonedMonster.GetComponent<MonsterBehaviourAbstractFSM>().Enable();
    }
    
    public void SpawnAtOnce()
    {
        for (int i = 0; i < maxSpawns; i++)
        {
        	SpawnEnemy();
        }
    }

    protected override void Die()
    {
        // Trigger all death events (for example opening doors)
        deathEvent.Invoke();

        // Set death flags used by KillableGridObject
        hasDied = true;
        isDying = true;

        spawnerAnimator.SetBool("dead", true);

        // Allows the death animation to play fully
        StartCoroutine(DeathSequence());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && player.isAttacking)
        {
            TakeDamage(player.damage);
        }
    }

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
                        Debug.Log("East");
                        east = false;
                    }
                    if (other.IsTouching(westCollider.GetComponent<Collider2D>()))
                    {
                        Debug.Log("west");
                        west = false;
                    }
                    if (other.IsTouching(northCollider.GetComponent<Collider2D>()))
                    {
                        Debug.Log("north");
                        north = false;
                    }
                    if (other.IsTouching(southCollider.GetComponent<Collider2D>()))
                    {
                        Debug.Log("south");
                        south = false;
                    }
                }
            }
            
        }
    }

    /// <summary>
    /// Allows the death animation to play fully before spawning items and destroying game object
    /// </summary>
    private IEnumerator DeathSequence()
    {
        // The animator does not change to death animation until the next frame
        yield return new WaitForEndOfFrame();

        // Get the length of the current clip which should be death animation
        //float deathAnimationLength = spawnerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //float deathAnimationLength = spawnerAnimator.GetCurrentAnimatorStateInfo(0).length;
        float deathAnimationLength = spawnerAnimator.GetNextAnimatorClipInfo(0)[0].clip.length;
        Debug.Log("Length: " + deathAnimationLength);

        yield return new WaitForSeconds(deathAnimationLength);

        SpawnItem();

        Destroy(this.gameObject);
    }

    public int numSpawns() {
    	return currSpawns;
    }

    public void KillSpawns() {
    	int i = 0;

    	while (i < spawnedMonsters.Count) {
    		GameObject obj = spawnedMonsters[i];

    		if (obj == null) {
    			spawnedMonsters.RemoveAt(i);
    		} else {
				KillableGridObject spawn = obj.GetComponent<KillableGridObject>();
    			spawn.TakeDamage(10000);
				spawnedMonsters.RemoveAt(i);
    		}
    	}

    	currSpawns = 0;
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

        currSpawns = spawnedMonsters.Count;
    }
}
