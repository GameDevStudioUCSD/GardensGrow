using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WindBossAI : KillableGridObject {
	public enum BossState { SpawningRocks, SpawningMonsters, Idle, Inhaling, Blowing };
	public RollingBoulder boulder;
    public GameObject portal;
    public GameObject spawnedMonster;
    public int idleFrames = 300;
    public int inhalingFrames = 400;
    public int blowingFrames = 300;

    private GameObject spawnedMonster1;
    private GameObject spawnedMonster2;

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
	private Animator animator;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		numRocks = 2;
		state = BossState.SpawningRocks;
		direction = Globals.Direction.South;
		animator = this.gameObject.GetComponent<Animator>();
		animator.SetInteger("State", 0);
		base.makeInvulnerable();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (state == BossState.SpawningRocks) {
			animator.SetInteger("State", 0);
			SpawnRocks();
			state = BossState.SpawningMonsters;
		}
		if (state == BossState.SpawningMonsters) {
			animator.SetInteger("State", 0);
			framesInState = 0;
			int integerDirection = UnityEngine.Random.Range(0, 4);
			int position;
			Vector3 newPosition;
			if (integerDirection == 0) {
				direction = Globals.Direction.South;
				position = UnityEngine.Random.Range(-4, 4);
				newPosition = new Vector3(position, 4.5f, 0.0f);
				animator.SetInteger("Direction", 0);
			}
			else if (integerDirection == 1) {
				direction = Globals.Direction.West;
				position = UnityEngine.Random.Range(-3, 3);
				newPosition = new Vector3(5.5f, position, 0.0f);
				animator.SetInteger("Direction", 2);
			}
			else if (integerDirection == 2) {
				direction = Globals.Direction.North;
				position = UnityEngine.Random.Range(-4, 4);
				newPosition = new Vector3(position, -4.5f, 0.0f);
				animator.SetInteger("Direction", 1);
			}
			else {
				direction = Globals.Direction.East;
				position = UnityEngine.Random.Range(-3, 3);
				newPosition = new Vector3(-5.5f, position, 0.0f);
				animator.SetInteger("Direction", 3);
			}

            // TODO: these slimes need to have their targeting and tilemap setup
            if (!spawnedMonster1) {
                spawnedMonster1 = (GameObject)Instantiate(spawnedMonster, new Vector3(-3, 0, 0), Quaternion.identity);
                PathFindingModule monsterPathFinding = spawnedMonster1.GetComponentInChildren<PathFindingModule>();
                monsterPathFinding.parameters.tileMap = Globals.tileMap;
                monsterPathFinding.parameters.target = Globals.player.gameObject;
            }
            if (!spawnedMonster2) {
                spawnedMonster2 = (GameObject)Instantiate(spawnedMonster, new Vector3(3, 0, 0), Quaternion.identity);
                PathFindingModule monsterPathFinding = spawnedMonster2.GetComponentInChildren<PathFindingModule>();
                monsterPathFinding.parameters.tileMap = Globals.tileMap;
                monsterPathFinding.parameters.target = Globals.player.gameObject;
            }

			this.transform.position = newPosition;
			state = BossState.Idle;
		}
		if (state == BossState.Idle) {
			framesInState++;
			if (framesInState > idleFrames) {
				//isInvulnerable = false;
				base.makeVulnerable();
				framesInState = 0;
				animator.SetInteger("State", 1);
				state = BossState.Inhaling;
			}
		}
		if (state == BossState.Inhaling) {
			framesInState++;
			if (framesInState > inhalingFrames) {
				//isInvulnerable = true;
				base.makeInvulnerable();
				framesInState = 0;
				animator.SetInteger("State", 2);
				state = BossState.Blowing;
			}
		}
		if (state == BossState.Blowing) {
			BlowRocks();
			framesInState++;
			if (framesInState > blowingFrames) {
				DestroyRocks();
				framesInState = 0;
				animator.SetInteger("State", 0);
				state = BossState.SpawningRocks;
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
		//numRocks+=2;
	}

	void BlowRocks() {
		foreach (KeyValuePair<BoulderLocation, RollingBoulder> kvp in rocks)
		{
            if (kvp.Value && !kvp.Value.isCrumbling) //check that boulder has not been destroyed
			    kvp.Value.StartRolling(direction);
		}
	}

	void DestroyRocks() {
		foreach (KeyValuePair<BoulderLocation, RollingBoulder> kvp in rocks)
		{
            if (kvp.Value && kvp.Value.gameObject)
			    Destroy(kvp.Value.gameObject);
		}
		rocks.Clear();
	}

    void OnCollisionEnter2D(Collision2D coll) {
        KillableGridObject killable = coll.gameObject.GetComponent<KillableGridObject>();
        if (killable)
            killable.TakeDamage(damage);
    }

    protected override void Die() {
        portal.SetActive(true);
        Globals.windBossBeaten = true;
        base.Die();
    }
}
