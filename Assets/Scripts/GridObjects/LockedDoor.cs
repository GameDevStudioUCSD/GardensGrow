using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public GameObject barrier;
	public bool unlockable;

	private Animator animator;
    private bool closed = false;

    private float x;
    private float y;
    private float z;

    void OnEnable()
    {
        PlayerGridObject p = FindObjectOfType<PlayerGridObject>();

        if (!p.itemsRePickUp) //check in player to save door/item info or not
        {
            x = this.gameObject.transform.position.x;
            y = this.gameObject.transform.position.y;
            z = this.gameObject.transform.position.z;

            closed = PlayerPrefsX.GetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                + "pos x" + x + "pos y" + y + "pos z" + z, closed);

            if (closed)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    void OnDisable()
    {

        PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                  + "pos x" + x + "pos y" + y + "pos z" + z, closed); //TODO put false into here, and save before building the game

    }
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
