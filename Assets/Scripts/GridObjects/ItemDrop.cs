using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ItemDrop : StaticGridObject {

	// The ID for the drop, 8 for health pickup, 0-7 for plants
	public int itemId;
	// The sound bite for when the player picks up the item
	public AudioClip clip;
	public bool permanent;
	public int lifeSpan;

	private int life;

	// Use this for initialization
	protected override void Start () {
		life = 0;
	}
	
	// Update is called once per frame
	void Update () {
		life++;
		if (permanent == false && life > lifeSpan) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
            if (player == null) return; // Ignore the player's other colliders (hacky)
			UIController controller = player.canvas;

            // Key is 9
			if (itemId == 9) {
				Globals.numKeys++;
				controller.UpdateUI();
			}
            // Health is 8
			else if (itemId == 8) {
				player.health++;
				if (player.health > 12)
					player.health = 12;
				controller.UpdateHealth(player.health);
			}
            // Seeds are 0 - 7
			else if (Globals.inventory[itemId] < 9) {
				Globals.inventory[itemId]++;
				controller.UpdateUI();
			}

			AudioSource.PlayClipAtPoint(clip, player.gameObject.transform.position);
			Destroy(this.gameObject);
		}
	}
}
