using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaBossAI : KillableGridObject {
	public GameObject[] spawners;
	public Vector3[] boatLocations;
	public GameObject boat;

	private List<GameObject> instantiatedBoats = new List<GameObject>();

	private int currentSpawnerIndex;

	public enum BossState { Dormant, Emerging, Emerged };
	public BossState state;

	// Use this for initialization
	void Start () {
		base.Start();
		currentSpawnerIndex = 0;
        StartCoroutine(waitForSpawns());
	}
	IEnumerator waitForSpawns()
    {
        yield return new WaitForSeconds(1);
        for(int i=0; i<spawners.Length; i++)
        {
            spawners[i].SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () {
		base.Update();
		int numSpawns = 0;
		EnemySpawner current;
		if (state == BossState.Dormant) {
			for (int i = 0; i < 4; i++) {
				if (i != currentSpawnerIndex) {
					current = (EnemySpawner)spawners[i].GetComponent<EnemySpawner>();
					numSpawns += current.numSpawns();
				}
			}
			if (numSpawns == 0) {
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
			state = BossState.Emerged;
		}
	}

	// Makes the boss "take over" a spawner and move to the new location"
	void Move () {
		// Destroy Boats
        for(int i=0; i<spawners.Length; i++)
        {
            spawners[i].SetActive(true);
            StartCoroutine(spawners[i].GetComponent<EnemySpawner>().spawnsAtOnce());
            StartCoroutine(waitForSpawns());
        }
		foreach(GameObject obj in instantiatedBoats)
        {
            PlatformGridObject thisBoat = obj.GetComponent<PlatformGridObject>();
			instantiatedBoats.Remove(obj);
            thisBoat.destructor();
            if (obj == null)
            {
                break;
            }
        }


		// Chooses a spawner index 0-3
		int spawnerToJumpTo = (int)Random.Range(0,4);

		// 'Disables' the spawner so that the boss can take over it
		spawners[spawnerToJumpTo].SetActive(false);

		// Moves the boss to the spawner's location
		Vector3 newPosition = spawners[spawnerToJumpTo].gameObject.transform.position;

		// 'Enables' the spawner that the boss was formerly at
		spawners[currentSpawnerIndex].SetActive(true);

		currentSpawnerIndex = spawnerToJumpTo;

		this.transform.position = newPosition;
	}

	public override bool TakeDamage(int dmg)
    {
    	if (state == BossState.Emerged)
    	{
        	//gameObject.GetComponent<Animation>().Play("Damaged");
        	//state = BossState.Dormant;
			state = BossState.Emerging;

        	Move();
        	return base.TakeDamage(dmg);
        }

        return false;
    }
}
