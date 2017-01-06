using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour {

	public GameObject fireball;
	public Vector3[] spawnLocations;

	private int indexToIgnore;

	// Use this for initialization
	void Start () {
		indexToIgnore = 0;
		SummonFireballs();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SummonFireballs() {
		for (int i = 0; i < spawnLocations.Length; i++) {
			if (i != indexToIgnore) {
				Instantiate(fireball, spawnLocations[i], Quaternion.identity);
			}
		} 

		//yield return new WaitForSeconds(0.3f);
	}
}
