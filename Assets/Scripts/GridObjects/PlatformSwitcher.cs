using UnityEngine;
using System.Collections;

public class PlatformSwitcher : MonoBehaviour {
	public Globals.Direction directionToSwitchTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.transform.position);
		if (other.CompareTag("Platform")) {
			Debug.Log("isplatform");
			PlatformGridObject platform = other.GetComponent<PlatformGridObject>();
			platform.changeDirection(directionToSwitchTo);
		}
	}
}
