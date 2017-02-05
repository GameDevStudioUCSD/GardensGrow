using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindBossAI : KillableGridObject {
	public enum BossState { SpawningRocks, SpawningMonsters, Idle, Inhaling, Blowing };
	public RollingBoulder boulder;
	public GameObject bossBody;

	private BossState state;
	private int numRocks;
	private List<RollingBoulder> rocks = new List<RollingBoulder>();
	private int framesInState;
	private Globals.Direction direction;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		numRocks = 5;
		state = BossState.SpawningRocks;
		direction = Globals.Direction.South;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (state == BossState.SpawningRocks) {
			Debug.Log("Spawning rocks");
			SpawnRocks();	
			state = BossState.SpawningMonsters;
		}
		if (state == BossState.SpawningMonsters) {
			Debug.Log("Spawning monsters");
			state = BossState.Idle;
			framesInState = 0;
			int integerDirection = Random.Range(0, 4);
			int position;
			Vector3 newPosition;
			if (integerDirection == 0) {
				direction = Globals.Direction.South;
				position = Random.Range(-4, 4);
				newPosition = new Vector3(position, -5.0f, 0.0f);
			}
			else if (integerDirection == 1) {
				direction = Globals.Direction.East;
				position = Random.Range(-3, 3);
				newPosition = new Vector3(-5.5f, position, 0.0f);
			}
			else if (integerDirection == 2) {
				direction = Globals.Direction.North;
				position = Random.Range(-4, 4);
				newPosition = new Vector3(position, 5.0f, 0.0f);
			}
			else {
				direction = Globals.Direction.West;
				position = Random.Range(-3, 3);
				newPosition = new Vector3(5.5f, position, 0.0f);
			}

			bossBody.transform.position = newPosition;
		}
		if (state == BossState.Idle) {
			Debug.Log("Idle");
			framesInState++;
			if (framesInState > 50) {
				state = BossState.Inhaling;
				framesInState = 0;
			}
		}
		if (state == BossState.Inhaling) {
			Debug.Log("Inhaling");
			framesInState++;
			if (framesInState > 20) {
				state = BossState.Blowing;
				framesInState = 0;
			}
		}
		if (state == BossState.Blowing) {
			Debug.Log("Blowing");
			framesInState++;
			if (framesInState == 1) {

			}
			if (framesInState > 50) {
				state = BossState.SpawningRocks;
				framesInState = 0;
			}
		}
	}

	void SpawnRocks() {
		for (int i = 0; i < numRocks; i++) {
			Vector3 dropPosition = new Vector3(Random.Range(-4, 4), Random.Range(-3, 3), 0.0f);
			RollingBoulder boulderObj = (RollingBoulder)Instantiate(boulder, dropPosition, Quaternion.identity);
			rocks.Add(boulderObj);
		}
		numRocks+=2;
	}

	void BlowRocks() {

	}
}
