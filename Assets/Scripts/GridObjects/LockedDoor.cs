using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public GameObject barrier;
	public bool unlockable;

	private Animator animator;

	// Use this for initialization
	void Start () {
		Debug.Log("test");
		animator = this.gameObject.GetComponent<Animator>();
	}

	void UnlockDoor() {
	Debug.Log("Opening door");
		animator.SetTrigger("Open");
		barrier.gameObject.SetActive(false);
	}

	void OnTriggerEnter2d(Collider2D other) {
		Debug.Log("enter");

		if (unlockable) {
			if (other.gameObject.tag == "Player") {
				Debug.Log("player");

				if (Globals.numKeys > 0) {
					Globals.numKeys--;
					Debug.Log("enough keys");

					PlayerGridObject player = other.GetComponent<PlayerGridObject>();
					UIController controller = player.canvas;

					controller.UpdateUI();

					UnlockDoor();
				}
			}
		}
	}
}
