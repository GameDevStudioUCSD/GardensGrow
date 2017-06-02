using UnityEngine;
using System.Collections;

public class WillowPlant : PlantGridObject {

	private int counter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		counter++;
		if (counter >= 200) {
			Debug.Log("Counter reached");
			Application.LoadLevel(0);
		}
	}
}
