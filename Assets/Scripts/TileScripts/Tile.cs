using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public bool isPatheable = true;
    public Vector2 worldPosition;

    // Position relative to TileMap (local position)
    public Vector2 gridPosition;

    // Path cost 
    // How costly it is to go through this tile
    // (e.x. 1 for normal, 2 for swamp, 5 for mountain)
    public int gCost = 1;

    // Heuristic cost 
    // (e.x. manhattan distance from goal)
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
