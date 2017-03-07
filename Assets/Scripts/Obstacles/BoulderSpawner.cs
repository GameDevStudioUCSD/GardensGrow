using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour {

	public GameObject boulder;
	public Globals.Direction direction;
	public int framesBetweenBoulders;
	public int startFrame;

    private RollingBoulder currentBoulder;

	private int currentFrame;

	// Use this for initialization
	void Start () {
		currentFrame = startFrame;
	}
	
	// Update is called once per frame
	void Update () {
		currentFrame++;
        if (currentBoulder) {
            currentBoulder.StartRolling(direction);
            currentBoulder = null;
        }
        if (currentFrame > framesBetweenBoulders) {
			currentFrame = 0;
			SummonBoulder();
		}
	}

	void SummonBoulder() {
		currentBoulder = ((GameObject)Instantiate(boulder, this.gameObject.transform.position, Quaternion.identity)).GetComponent<RollingBoulder>();
	}
}
