using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ItemDrop : StaticGridObject {

	// The ID for the drop, 9 for health pickup, 0-8 for plants
	public int itemId;
	// The sound bite for when the player picks up the item
	public AudioClip clip;
	public bool permanent;
	public int lifeSpan;

	private int life;

	// Use this for initialization
	void Start () {
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
			UIController controller = player.canvas;

			if (itemId == 9) {
				player.health++;
				if (player.health > 12)
					player.health = 12;
				controller.UpdateHealth(player.health);
			}
			else if (Globals.inventory[itemId] < 9) {
				Globals.inventory[itemId]++;
				controller.UpdateUI();
			}

			AudioSource.PlayClipAtPoint(clip, player.gameObject.transform.position);
			Destroy(this.gameObject);
		}
	}
}
