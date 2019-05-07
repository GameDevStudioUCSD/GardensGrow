using UnityEngine;
using System.Collections;

public class BossLockedDoor : MonoBehaviour {

	public Sprite lava;
	public Sprite wind;
	public Sprite cave;
    public GameObject barrier;
    private Animator animator;
    public bool opened;

    // Use this for initialization
    void Start () {
        opened = false;
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        if (Globals.caveBossBeaten) {
        	renderer.sprite = cave;
        }
        else if (Globals.windBossBeaten) {
        	renderer.sprite = wind;
        }
        else if (Globals.lavaBossBeaten) {
        	renderer.sprite = lava;
        }
    }

    void OpenDoor() {
        animator.SetTrigger("Open");
        barrier.gameObject.SetActive(false);
        opened = true;
    }

    void CloseDoor() {
        animator.SetTrigger("Close");
        barrier.gameObject.SetActive(true);
        opened = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (Globals.lavaBossBeaten && Globals.windBossBeaten && Globals.caveBossBeaten) {
                if(!opened) OpenDoor();
            }
        }
    }

    public void Toggle() {
        if (opened) {
            CloseDoor();
        }
        else {
            OpenDoor();
        }
    }
}

