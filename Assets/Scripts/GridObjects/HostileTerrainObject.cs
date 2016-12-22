using UnityEngine;
using System.Collections;

public class HostileTerrainObject : TerrainObject {
	public int damage;
	private int framesPerHit = 30;
	private int currentFrame = 0;

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			if (player.onPlatform == false) {
				currentFrame = (currentFrame + 1) % framesPerHit;
				if (currentFrame == 0)
					player.TakeDamage(damage);
			}
		}
	}
}
