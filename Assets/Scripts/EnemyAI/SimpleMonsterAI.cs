using UnityEngine;
using System.Collections;

public class SimpleMonsterAI : MonoBehaviour {

    // searching how far from goal (heuristic) and how far from start (path cost)

	// Use this for initialization
	void Start () {
        

        // Priority Queue that sorts lowest cost to highest cost
        // From start position get all successor nodes
        // Calculate total cost for each node
        // add these nodes into PQ
        // While PQ not empty
        // Pop node
        // Check if goal
        // get all successors
        // calculate
        // push into PQ
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int heuristic(Vector2 currentPosition, Vector2 goalPosition) 
    {
        return (int)Vector2.Distance(currentPosition, goalPosition);
    }

    int pathCost(Vector2 currentPosition, Vector2 startPosition) 
    {
        // TODO:
        return 0;
    }
}
