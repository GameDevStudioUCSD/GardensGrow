using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindBossAI : KillableGridObject {
	public enum BossState { SpawningRocks, SpawningMonsters, Idle, Inhaling, Blowing };
	public RollingBoulder boulder;

	private BossState state;
	private int numRocks;
	private List<RollingBoulder> rocks = new List<RollingBoulder>();

	// Use this for initialization
	protected override void Start () {
		base.Start();
		numRocks = 5;
		state = BossState.SpawningRocks;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (state == BossState.SpawningRocks) {
			SpawnRocks();	
			state = BossState.SpawningMonsters;
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
}
