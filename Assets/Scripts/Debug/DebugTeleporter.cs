using UnityEngine;
using System.Collections;

/// <summary>
/// Teleports target object to location of this object.
/// </summary>
public class DebugTeleporter : MonoBehaviour {

    public GameObject target;

    public bool teleport = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(teleport)
        {
            TeleportTarget();
            teleport = false;
        }
	}

    void TeleportTarget()
    {
        target.transform.position = this.transform.position;
    }
}
