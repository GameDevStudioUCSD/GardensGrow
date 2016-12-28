using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaBossAI : KillableGridObject {
	public GameObject[] spawners;
	public Vector3[] boatLocations;
	public GameObject boat;
	//public BoxCollider2D collider;
	//public SpriteRenderer sprite;

	private List<GameObject> instantiatedBoats = new List<GameObject>();

	private int currentSpawnerIndex;

	public enum BossState { Dormant, Emerging, Emerged, Enraged };
	public BossState state;

	// Use this for initialization
	void Start () {
		base.Start();
		currentSpawnerIndex = -1; // so boss randomly spawns in any corner
		state = BossState.Emerging;
	}

	// Update is called once per frame
	void Update () {
		base.Update();
		int numSpawns = 0;
		EnemySpawner current;
		if (state == BossState.Dormant) {
			for (int i = 0; i < 4; i++) {
				if (i != currentSpawnerIndex) {
					current = spawners[i].GetComponent<EnemySpawner>();
					numSpawns += current.numSpawns();
				}
			}

			if (numSpawns < 4) {
				state = BossState.Emerging;
			}
		}
		if (state == BossState.Emerging) {
			int evenOrOdd = (int)Random.Range(0,2);
			for (int i = 0; i < 4; i++) {
				if (i % 2 == evenOrOdd) {
					instantiatedBoats.Add((GameObject)Instantiate(boat, boatLocations[i], Quaternion.identity));
				}
			}
			Emerge();
			state = BossState.Emerged;
			isInvulnerable = false;
		}
	}

	void SpawnEnemies () {
		// Spawn enemies
        for(int i=0; i<spawners.Length; i++)
        {
            spawners[i].GetComponent<EnemySpawner>().SpawnAtOnce();
        }
	}

	void Hide () {
		// Hide boss
		isInvulnerable = true;
		state = BossState.Dormant;
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = false;
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = false;

		// 'Enables' the spawner that the boss was formerly at
		spawners[currentSpawnerIndex].SetActive(true);

		// Destroy Boats
		int i = 0;
		while (i < instantiatedBoats.Count)
        {
        	GameObject curBoat = instantiatedBoats[i];

        	if (curBoat) {
				PlatformGridObject thisBoat = curBoat.GetComponent<PlatformGridObject>();
				instantiatedBoats.RemoveAt(i);
				thisBoat.Destructor();
        	} else {
        		i++;
        	}
        }
	}

	// Makes the boss "take over" a spawner and move to the new location"
	void Emerge () {
		// Chooses a spawner index 0-3
		int newSpawnerIndex = currentSpawnerIndex;

		// Guarantees spawn in new location
		while (newSpawnerIndex == currentSpawnerIndex)
		{
			newSpawnerIndex = (int)Random.Range(0,4);
		}

		// temp hack TOOD: remove later
		newSpawnerIndex = 0;

		// 'Disables' the spawner so that the boss can take over it
		spawners[newSpawnerIndex].SetActive(false);

		// Moves the boss to the spawner's location
		Vector3 newPosition = spawners[newSpawnerIndex].gameObject.transform.position;
		newPosition.y++;

		currentSpawnerIndex = newSpawnerIndex;

		this.transform.position = newPosition;

		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = true;
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = true;
	}

	public override bool TakeDamage(int dmg)
    {
    	if (state == BossState.Emerged ||
    		state == BossState.Enraged)
    	{
        	//gameObject.GetComponent<Animation>().Play("Damaged");
        	//state = BossState.Dormant;
			

			Hide();
			SpawnEnemies();
        	return base.TakeDamage(dmg);
        }

        return false;
    }
}
