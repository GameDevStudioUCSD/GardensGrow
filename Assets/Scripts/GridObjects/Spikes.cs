using UnityEngine;
using System.Collections;

public class Spikes : TerrainObject {
	public int damage = 2;
	public int framesPerHit = 50;
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
	}
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log(currentFrame);

			currentFrame++;
			if (currentFrame > framesPerHit) {
				PlayerGridObject player = other.GetComponent<PlayerGridObject>();
				if (player.onPlatform == false && !striked) {
					player.TakeDamage(damage);
					//player.gameObject.transform.position = Globals.spawnLocation;
                    anim.SetBool("Hit", true);
                    striked = true;
                    StartCoroutine(wait());
				}
				currentFrame = 0;
			}
		}
	}
    //method for animation ease
    IEnumerator wait()
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
}
