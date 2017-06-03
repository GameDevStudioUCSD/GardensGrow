using UnityEngine;
using System.Collections;

public class HealingFountain : MonoBehaviour {

	public AudioClip clip;

	private int framecounter;

	// Use this for initialization
	void Start () {
		framecounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
            if (player == null) return; // Ignore the player's other colliders (hacky)

            framecounter++;

            if (framecounter > 50) {
				UIController controller = player.canvas;
				if (player.health < 12) {
					player.health++;
					controller.UpdateHealth(player.health);
					AudioSource.PlayClipAtPoint(clip, player.gameObject.transform.position);
				}
				framecounter = 0;
			}
		}
	}
}


