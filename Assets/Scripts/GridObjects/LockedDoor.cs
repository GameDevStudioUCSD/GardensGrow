using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public GameObject barrier;
	public bool unlockable;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.gameObject.GetComponent<Animator>();
	}

	void UnlockDoor() {
		animator.SetBool("Opening", true);
		barrier.gameObject.SetActive(false);
	}

	void OnTriggerEnter2d(Collider other) {
		if (unlockable) {
			if (other.gameObject.tag == "Player") {
				if (Globals.numKeys > 0) {
					Globals.numKeys--;
					PlayerGridObject player = other.GetComponent<PlayerGridObject>();
					UIController controller = player.canvas;

					controller.UpdateUI();

					UnlockDoor();
				}
			}
		}
	}
}
