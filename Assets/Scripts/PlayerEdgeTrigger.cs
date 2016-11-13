using UnityEngine;
using System.Collections.Generic;

public class PlayerEdgeTrigger : MonoBehaviour
{
	public bool isTriggered;
	Collider2D other;   	// the other collider
	private List<KillableGridObject> killList = new List<KillableGridObject> ();

	void Update ()
	{
		if (isTriggered && !other) {
			isTriggered = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.isTrigger)
			isTriggered = true;

		KillableGridObject killable = other.GetComponent<KillableGridObject> ();
		if (killable != null) {
			killList.Add (killable);
		}

		this.other = other;
	}

	void OnTriggerStay2D (Collider2D other)
	{ 
		if (!other.isTrigger)
			isTriggered = true;

		this.other = other;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (!other.isTrigger)
			isTriggered = false;

		KillableGridObject killable = other.GetComponent<KillableGridObject> ();
		if (killable != null) {
			killList.Remove (killable);
		}

		this.other = other;
	}

	public List<KillableGridObject> getList ()
	{
		return killList;
	}

	// removes a killable grid object from the list
	public void removeFromList (KillableGridObject killable)
	{
		if (killable != null)
			killList.Remove (killable);
	}
}
