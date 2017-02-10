using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WindBossAI : KillableGridObject {
	public enum BossState { SpawningRocks, SpawningMonsters, Idle, Inhaling, Blowing };
	public RollingBoulder boulder;
	public GameObject bossBody;

	public struct BoulderLocation : IComparable <BoulderLocation> {
		public Vector3 location;

		public BoulderLocation (Vector3 newLocation) {
			location = newLocation;
		}

		public int CompareTo(BoulderLocation other) {
			if (this.location.x < other.location.x) {
    			return -1;
    		}
			else if (this.location.x > other.location.x) {
    			return 1;
    		}
			else if (this.location.y < other.location.y) {
    			return -1;
    		}
			else if (this.location.y > other.location.y) {
    			return 1;
    		}
			else if (this.location.z < other.location.z) {
    			return -1;
    		}
			else if (this.location.z > other.location.z) {
    			return 1;
    		}
    		else {
    			return 0;
    		}
		}
	}

	private BossState state;
	private int numRocks;
	private SortedList<BoulderLocation, RollingBoulder> rocks = new SortedList<BoulderLocation, RollingBoulder>();
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
			SpawnRocks();	
			state = BossState.SpawningMonsters;
		}
		if (state == BossState.SpawningMonsters) {
			Debug.Log("Spawning monsters");
			state = BossState.Idle;
			framesInState = 0;
			int integerDirection = UnityEngine.Random.Range(0, 4);
			int position;
			Vector3 newPosition;
			if (integerDirection == 0) {
				direction = Globals.Direction.North;
				position = UnityEngine.Random.Range(-4, 4);
				newPosition = new Vector3(position, -5.0f, 0.0f);
			}
			else if (integerDirection == 1) {
				direction = Globals.Direction.East;
				position = UnityEngine.Random.Range(-3, 3);
				newPosition = new Vector3(-5.5f, position, 0.0f);
			}
			else if (integerDirection == 2) {
				direction = Globals.Direction.South;
				position = UnityEngine.Random.Range(-4, 4);
				newPosition = new Vector3(position, 5.0f, 0.0f);
			}
			else {
				direction = Globals.Direction.West;
				position = UnityEngine.Random.Range(-3, 3);
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
			BlowRocks();
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
			Vector3 dropPosition = new Vector3(UnityEngine.Random.Range(-4, 5), UnityEngine.Random.Range(-3, 4), 0.0f);
			BoulderLocation newLocation = new BoulderLocation(dropPosition);

			if (!rocks.ContainsKey(newLocation)) {
				RollingBoulder boulderObj = (RollingBoulder)Instantiate(boulder, dropPosition, Quaternion.identity);
				rocks.Add(newLocation, boulderObj);
			}
		}
		numRocks+=2;
	}

	void BlowRocks() {
		foreach (KeyValuePair<BoulderLocation, RollingBoulder> kvp in rocks)
		{
			kvp.Value.startRolling(direction);
		}
	}

	void DestroyRocks() {
		foreach (KeyValuePair<BoulderLocation, RollingBoulder> kvp in rocks)
		{
			Destroy(kvp.Value.gameObject);
		}
		rocks.Clear();
	}
}
