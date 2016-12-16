using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// An object's attack collider script, used to detect enemies that can be attacked.
/// </summary>
public class AttackCollider : MonoBehaviour {

    // Collider this script is for
    private Collider2D collider;

    private List<KillableGridObject> killList = new List<KillableGridObject>();

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.isTrigger)
        {
            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if(killable)
            {
                killList.Add(killable);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!other.isTrigger)
        {
            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if(killable && killList.Contains(killable))
            {
                killList.Remove(killable);
            }
        }
    }

}
