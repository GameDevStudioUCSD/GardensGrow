using UnityEngine;
using System.Collections;

public class TeleportBackLocation : MonoBehaviour {

	public Vector3 RespawnLocation;
	public bool VolcanoBoss;
	public bool TornadoBoss;
	public bool JellyFishBoss;

	void Start() {
		if (VolcanoBoss && !Globals.lavaBossBeaten) {
			this.gameObject.SetActive(false);
		}
		if (TornadoBoss && !Globals.windBossBeaten) {
			this.gameObject.SetActive(false);
		}
		if (JellyFishBoss && !Globals.caveBossBeaten) {
			this.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player"))
        {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			player.gameObject.transform.position = RespawnLocation;
        }
	}
}
