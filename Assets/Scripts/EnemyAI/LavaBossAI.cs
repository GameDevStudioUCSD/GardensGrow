using UnityEngine;
using System.Collections;

public class LavaBossAI : KillableGridObject {
	public EnemySpawner[] spawners;
	public Vector3[] boatLocations;
	public GameObject boat;

	private int currentSpawnerIndex;

	public enum BossState { Dormant, Emerging, Emerged };
	public BossState state;

	// Use this for initialization
	void Start () {
		base.Start();
		currentSpawnerIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		int numSpawns = 0;

		if (state == BossState.Dormant) {
			for (int i = 0; i < 4; i++) {
				if (i != currentSpawnerIndex) {
					numSpawns += spawners[i].numSpawns();
				}
			}
			if (numSpawns == 0) {
				state = BossState.Emerging;
			}
		}
		if (state == BossState.Emerging) {
			for (int i = 0; i < 4; i++) {
				Instantiate(boat, boatLocations[i], Quaternion.identity);
			}
			state = BossState.Emerged;
		}
	}

	// Makes the boss "take over" a spawner and move to the new location"
	void Move () {
		// Chooses a spawner index 0-3
		int spawnerToJumpTo = (int)Random.Range(0,4);
		spawners[currentSpawnerIndex].enabled = true;
		currentSpawnerIndex = spawnerToJumpTo;

		Vector3 newPosition = spawners[currentSpawnerIndex].gameObject.transform.position;

		// 'Disables' the spawner so that the boss can take over it
		spawners[currentSpawnerIndex].enabled = false;

		this.transform.position = newPosition;
	}

	public override bool TakeDamage(int dmg)
    {
    	if (state == BossState.Emerged)
    	{
        	//gameObject.GetComponent<Animation>().Play("Damaged");
        	state = BossState.Dormant;
        	return base.TakeDamage(dmg);
        }

        return false;
    }
}
