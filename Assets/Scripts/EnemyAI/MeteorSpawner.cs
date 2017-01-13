using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour {

	public GameObject fireball;
	public Vector3[] spawnLocations;

	public int framesBetweenMeteors;

	private int indexToIgnore;
	private int currentFrame;

	// Use this for initialization
	void Start () {
		indexToIgnore = 0;
		currentFrame = 0;
		SummonFireballs();
	}
	
	// Update is called once per frame
	void Update () {
		currentFrame = (currentFrame + 1) % framesBetweenMeteors;
		if (currentFrame == 0) {
			SummonFireballs();
			indexToIgnore = (indexToIgnore + 1) % spawnLocations.Length;
		}
	}

	void SummonFireballs() {
		for (int i = 0; i < spawnLocations.Length; i++) {
			if (i != indexToIgnore) {
				Instantiate(fireball, spawnLocations[i], Quaternion.identity);
			}
		} 
	}
}
