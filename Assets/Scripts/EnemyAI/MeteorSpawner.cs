using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour {

	public GameObject fireball;
	public Vector3[] spawnLocations;

	public int framesBetweenMeteors;

	private int indexToIgnore;
	private int currentFrame;

	private bool triggered;

	// Use this for initialization
	void Start () {
		indexToIgnore = 0;
		currentFrame = 0;
		triggered = false;
		//SummonFireballs();
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered == true)
		{
			currentFrame = (currentFrame + 1) % framesBetweenMeteors;
			if (currentFrame == 0) {
				SummonFireballs();
				indexToIgnore = (indexToIgnore + 1) % spawnLocations.Length;
			}
		}
	}

	void SummonFireballs() {
		for (int i = 0; i < spawnLocations.Length; i++) {
			if (i != indexToIgnore) {
				Instantiate(fireball, spawnLocations[i], Quaternion.identity);
			}
		} 
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			triggered = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			triggered = false;
		}
	}
}
