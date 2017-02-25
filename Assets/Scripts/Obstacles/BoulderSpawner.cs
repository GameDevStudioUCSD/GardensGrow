using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour {

	public GameObject boulder;
	public Vector3  spawningLocation;
	public Globals.Direction direction;
	public int framesBetweenBoulders;

    private RollingBoulder currentBoulder;

	private int currentFrame;

	// Use this for initialization
	void Start () {
		currentFrame = 0;
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
		currentBoulder = ((GameObject)Instantiate(boulder, spawningLocation, Quaternion.identity)).GetComponent<RollingBoulder>();
	}
}
