using UnityEngine;
using System.Collections;

public class BossLockedDoor : MonoBehaviour {

	public Sprite lava;
	public Sprite wind;
	public Sprite cave;
    public GameObject barrier;
    private Animator animator;
    private bool closed;

    // Use this for initialization
    void Start () {
        closed = true;
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (Globals.caveBossBeaten) {
        	renderer.sprite = lava;
        }
        else if (Globals.windBossBeaten) {
        	renderer.sprite = wind;
        }
        else if (Globals.lavaBossBeaten) {
        	renderer.sprite = cave;
        }
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
        if (other.gameObject.tag == "Player") {
            if (Globals.lavaBossBeaten && Globals.windBossBeaten && Globals.caveBossBeaten) {
                OpenDoor();
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

