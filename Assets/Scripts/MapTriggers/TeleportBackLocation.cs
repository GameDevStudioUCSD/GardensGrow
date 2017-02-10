using UnityEngine;
using System.Collections;

public class TeleportBackLocation : MonoBehaviour {

	public Vector3 RespawnLocation;
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player"))
        {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			player.gameObject.transform.position = RespawnLocation;
        }
	}
}
