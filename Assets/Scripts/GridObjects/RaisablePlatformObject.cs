using UnityEngine;
using System.Collections;

public class RaisablePlatformObject : MonoBehaviour {

	bool raised = false;
	public GameObject bossEntrance;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (raised == true) {
			if (col.gameObject.CompareTag("Player")) {
	            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
	            player.platforms++;
	        }
        }
    }

	void OnTriggerExit2D(Collider2D col) {
		if (raised == true) {
			if (col.gameObject.CompareTag("Player")) {
	            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
	            player.platforms--;
	        }
        }
    }

    public void Toggle() {
    	if (raised == false) {
    		raised = true;
    		bossEntrance.SetActive(true);
    		animator.SetTrigger("RaisePlatform");
    	}
    	else {
    		raised = false;
    		bossEntrance.SetActive(false);
    	}
    }
}
