using UnityEngine;
using System.Collections.Generic;

public class SimpleMonsterAI : MonoBehaviour {

    public TileMap tileMap;

    public EnemyGridObject entity;
    public float updateRate = 1;
    [Range(0,1)]
    public float chanceToChangeDirection;
    private Globals.Direction movementDirection;
    // searching how far from goal (heuristic) and how far from start (path cost)

    bool flag = false;

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
        InvokeRepeating("RandomMovement", 0, 1.0f/updateRate);

	}

	// Update is called once per frame
	void Update () {
        Debug.DrawRay(transform.position, Globals.DirectionToVector(entity.direction));

    }

    void RandomMovement()
    {
        if(Random.value < chanceToChangeDirection)
        {
            int m = Random.Range(0, 4);
            movementDirection = (Globals.Direction)m;
        }
        entity.Move(movementDirection);
    }

    
}
