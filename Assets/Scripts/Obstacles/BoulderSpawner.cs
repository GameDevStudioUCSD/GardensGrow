using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour {

	public GameObject boulder;
	public Vector3  spawningLocation;
	public Globals.Direction direction;
	public int framesBetweenBoulders;

	private int currentFrame;
	private RollingBoulder currentBoulder;

	// Use this for initialization
	void Start () {
		currentFrame = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentFrame++;
		if (currentFrame > framesBetweenBoulders) {
			currentFrame = 0;
			SummonBoulder();
		}
	}

	void SummonBoulder() {
		if (currentBoulder != null) {
			currentBoulder.PublicDeath();
		}
		currentBoulder = (RollingBoulder)Instantiate(boulder, spawningLocation, Quaternion.identity);
		currentBoulder.StartRolling(direction);
	}
}
