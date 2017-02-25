using UnityEngine;
using System.Collections;

// Updates the player's respawn location
public class RespawnLocation : StaticGridObject {
	public Vector3 spawnLocation;
    public AudioClip music;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			updateSpawnLocation();
            AudioSource musicSource = Globals.tileMap.GetComponent<AudioSource>();
            if (music && musicSource.clip != music) {
                musicSource.clip = music;
                musicSource.Play();
            }
        }
	}

	protected void updateSpawnLocation () {
		Globals.spawnLocation = this.spawnLocation;
	}
}