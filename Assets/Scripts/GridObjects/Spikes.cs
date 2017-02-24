using UnityEngine;
using System.Collections;

public class Spikes : TerrainObject {
	public int damage = 2;
	public int framesPerHit = 50;

	public bool toggleable = false;
	public bool spikesUp = false;

	private int currentFrame = 0;
    private Animator anim;
    private bool striked = false;

	public bool activeCollider;

	// Use this for initialization
	protected override void Start () {
		base.Start();
        anim = this.gameObject.GetComponent<Animator>();
		if (!activeCollider) {
			BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
			thisCollider.enabled = false;
			Destroy(transform.GetComponent<Rigidbody>());
		}

		if (toggleable) {
			anim.SetBool("Hit", false);
			anim.SetBool("SpikesUp", spikesUp);
		}
	}
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();

			if (toggleable == false) {
				currentFrame++;
				if (currentFrame > framesPerHit) {
					if (player.onPlatform == false && !striked) {
						player.TakeDamage(damage);
						//player.gameObject.transform.position = Globals.spawnLocation;
	                    anim.SetBool("Hit", true);
	                    striked = true;
	                    StartCoroutine(Wait());
					}
					currentFrame = 0;
				}
			}
			else {
				if (spikesUp) {
					player.TakeDamage(damage);
					player.gameObject.transform.position = Globals.spawnLocation;
				}
			}
		}
	}
    //method for animation ease
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.33f);
        anim.SetBool("Hit", false);
        yield return new WaitForSeconds(.5f);
        striked = false;
    }
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			currentFrame = 0;
		}
	}

	public void Toggle() {
		if (spikesUp) {
			spikesUp = false;
			anim.SetBool("SpikesUp", false);
		}
		else {
			spikesUp = true;
			anim.SetBool("SpikesUp", true);
		}
	}
}
