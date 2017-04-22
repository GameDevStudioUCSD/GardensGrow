using UnityEngine;
using System.Collections;

public class RaisablePlatformObject : MonoBehaviour {

	bool raised = false;
	public GameObject bossEntrance;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (raised == true) {
			if (col.gameObject.CompareTag("Player")) {
	            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
	            player.onPlatform = true;
	        }
        }
    }

	void OnTriggerExit2D(Collider2D col) {
		if (raised == true) {
			if (col.gameObject.CompareTag("Player")) {
	            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
	            player.onPlatform = false;
	        }
        }
    }

    public void Toggle() {
    	if (raised == false) {
    		raised = true;
    		bossEntrance.SetActive(true);
    	}
    	else {
    		raised = false;
    		bossEntrance.SetActive(false);
    	}
    }
}
