using UnityEngine;
using System.Collections;

// Updates the player's respawn location
public class RespawnLocation : StaticGridObject {
	public Vector3 spawnLocation;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			updateSpawnLocation();
		}
	}

	protected void updateSpawnLocation () {
		Globals.spawnLocation = this.spawnLocation;
	}
}