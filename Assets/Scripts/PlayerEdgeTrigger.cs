using UnityEngine;
using System.Collections.Generic;

public class PlayerEdgeTrigger : MonoBehaviour {
	public bool isTriggered;
	private List<KillableGridObject> killList = new List<KillableGridObject>();

	void OnTriggerEnter2D(Collider2D other) {
        if (!other.isTrigger)
		isTriggered = true;

		KillableGridObject killable = other.GetComponent<KillableGridObject>();
        if (killable != null)
        {
        	killList.Add(killable);
    	}
	}

	void OnTriggerStay2D(Collider2D other) { 
        if (!other.isTrigger)
		isTriggered = true;
	}

	void OnTriggerExit2D(Collider2D other) {
        if(!other.isTrigger)
		isTriggered = false;

		KillableGridObject killable = other.GetComponent<KillableGridObject>();
        if (killable != null)
        {
       		killList.Remove(killable);
    	}
	}

	public List<KillableGridObject> getList()
	{
		return killList;
	}
}
