using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public GameObject barrier;
	public bool unlockable;

	private Animator animator;
	private bool closed;

	// Use this for initialization
	void Start () {
		closed = true;
		animator = this.gameObject.GetComponent<Animator>();
	}

	void OpenDoor() {
		animator.SetTrigger("Open");
		barrier.gameObject.SetActive(false);
		closed = false;
	}

	void CloseDoor() {
		animator.SetTrigger("Close");
		barrier.gameObject.SetActive(true);
		closed = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (unlockable) {
			if (other.gameObject.tag == "Player") {

				if (Globals.numKeys > 0) {
					Globals.numKeys--;

					PlayerGridObject player = other.GetComponent<PlayerGridObject>();
					UIController controller = player.canvas;

					controller.UpdateUI();

					OpenDoor();
				}
			}
		}
	}

	public void Toggle() {
		if (closed) {
			OpenDoor();
		}
		else {
			CloseDoor();
		}
	}
}
