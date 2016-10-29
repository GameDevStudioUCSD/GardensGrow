using UnityEngine;
using System.Collections;

public class SimpleMonsterAI : MonoBehaviour {

    public EnemyGridObject entity;
    public float updateRate = 1;
    [Range(0,1)]
    public float chanceToChangeDirection;
    private Globals.Direction movementDirrection;
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
            movementDirrection = (Globals.Direction)m;
        }
        entity.Move(movementDirrection);
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
