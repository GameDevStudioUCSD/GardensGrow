using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// An object's attack collider script, used to detect enemies that can be attacked.
/// </summary>
public class AttackCollider : MonoBehaviour {

    private List<KillableGridObject> killList = new List<KillableGridObject>();
    
	// Update is called once per frame
	void Update () {
	}

    public List<KillableGridObject> GetKillList()
    {
        return killList;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.isTrigger || other.GetComponent<PlantGridObject>())
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
        if(!other.isTrigger || other.GetComponent<PlantGridObject>())
        {
            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if(killable && killList.Contains(killable))
            {
                killList.Remove(killable);
            }
        }
    }

}
