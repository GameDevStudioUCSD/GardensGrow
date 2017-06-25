using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public GameObject barrier;
	public bool unlockable;

	private Animator animator;
    private bool closed = true;

    private float x;
    private float y;
    private float z;

    void OnEnable()
    {
        PlayerGridObject p = FindObjectOfType<PlayerGridObject>();

        //makes sure we have an animator
        if (!animator)
        {
            animator = this.gameObject.GetComponent<Animator>();
        }

        x = this.gameObject.transform.position.x;
        y = this.gameObject.transform.position.y;
        z = this.gameObject.transform.position.z;

        closed = PlayerPrefsX.GetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
            + "pos x" + x + "pos y" + y + "pos z" + z + "door");

        if (closed)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }

    }
    void OnDisable()
    {
        if (Globals.restartSaveState)
        {
            PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
             + "pos x" + x + "pos y" + y + "pos z" + z + "door", false);
        }
        else
        {
            PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
          + "pos x" + x + "pos y" + y + "pos z" + z + "door", closed);
        }

    }
    // Use this for initialization
    void Start () {
        //used to deterministically save and load the closed bool
        x = this.gameObject.transform.position.x;
        y = this.gameObject.transform.position.y;
        z = this.gameObject.transform.position.z;

        //closed = true;
		animator = this.gameObject.GetComponent<Animator>();
	}

	void OpenDoor() {
        //null check
        if (animator)
        {
            animator.SetTrigger("Open");
        }
		barrier.gameObject.SetActive(false);
		closed = false;
	}

	void CloseDoor() {
        //null check
        if (animator)
        {
            animator.SetTrigger("Close");
        }
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
