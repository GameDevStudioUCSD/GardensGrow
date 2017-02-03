using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindBossAI : KillableGridObject {
	public enum BossState { SpawningRocks, SpawningMonsters, Idle, Inhaling, Blowing };
	private BossState state;
	private List<RollingBoulder> rocks = new List<RollingBoulder>();

	// Use this for initialization
	protected override void Start () {
		base.Start();
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
		Debug.Log("spawn rocks");
	}
}
