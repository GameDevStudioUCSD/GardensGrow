using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ItemDrop : StaticGridObject {

	public int plantId;
	public AudioClip clip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			UIController controller = player.canvas;
			Globals.inventory[plantId]++;
			Debug.Log("Id: " + plantId + ", Amount: " + Globals.inventory[plantId]);
			controller.UpdateUI();
			AudioSource.PlayClipAtPoint(clip, player.gameObject.transform.position);
			Destroy(this.gameObject);
		}
	}
}
