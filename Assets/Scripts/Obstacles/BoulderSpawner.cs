using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour {

	public GameObject boulder;
	public Globals.Direction direction;
	public int framesBetweenBoulders;
	public int startFrame;

    private RollingBoulder currentBoulder;

	private int currentFrame;
    private bool active = true;

	// Use this for initialization
	void Start () {
		currentFrame = startFrame;
	}

    // Update is called once per frame
    void Update() {
        if (!Globals.canvas.dialogue) {
            currentFrame++;
            if (currentBoulder) {
                currentBoulder.StartRolling(direction);
                currentBoulder = null;
            }
            if (active && currentFrame > framesBetweenBoulders) {
                currentFrame = 0;
                SummonBoulder();
            }
        }
	}

	void SummonBoulder() {
		currentBoulder = ((GameObject)Instantiate(boulder, this.gameObject.transform.position, Quaternion.identity)).GetComponent<RollingBoulder>();
	}

    //OnTrigger stuff makes it so boulders can't spawn on top of the player
    void OnTriggerEnter2D(Collider2D other) {
        PlayerGridObject player = other.GetComponent<PlayerGridObject>();
        if (player) {
            active = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        PlayerGridObject player = other.GetComponent<PlayerGridObject>();
        if (player) {
            currentFrame = framesBetweenBoulders - 20;
            active = true;
        }
    }
}
