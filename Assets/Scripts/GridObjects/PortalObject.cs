using UnityEngine;
using System.Collections;

public class PortalObject : RespawnLocation {
	// 0 = Main Menu
	// 1 = Overworld
	// 2 = Lava Dungeon
	// 3 = Lava Dungeon Boss

	public int levelToLoad;

	void OnTriggerEnter2D (Collider2D other) {

		if (other.gameObject.CompareTag("Player"))
        {
        	base.updateSpawnLocation();
        	Application.LoadLevel(levelToLoad);
        }
	}
}
