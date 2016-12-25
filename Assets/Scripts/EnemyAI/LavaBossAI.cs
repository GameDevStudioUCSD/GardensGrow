using UnityEngine;
using System.Collections;

public class LavaBossAI : KillableGridObject {
	public EnemySpawner[] spawners;

	private int currentSpawnerIndex;


	// Use this for initialization
	void Start () {
		base.Start();
		currentSpawnerIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
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
}
