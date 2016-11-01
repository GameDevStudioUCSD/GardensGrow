using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public bool isPatheable = true;

    // Path cost 
    // How costly it is to go through this tile
    // (e.x. 1 for normal, 2 for swamp, 5 for mountain)
    public int gCost = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
