using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Path Finding component to the high level AI state machine.
/// Call Step() to progress this component one tick.
/// 
/// A path is a list of directions from start to the end.
/// Uses the A Star algorithm to create this path.
/// Getting from one tile to another tile involves calling Move() 32 times.
/// Each Move() call will be called a step, it takes 32 steps to move tile length.
/// </summary>
public class PathFindingBehaviour : PathFindingBehaviourAbstractFSM {

    [Header("Path Finding Components")]
    public TileMap tileMap;
    public MoveableGridObject creature;
    public GameObject target;

    protected Transform creatureTransform;

    [Header("Path Finding Parameters")]
    public int stepAmount = 32;
    public float delayBetweenSteps = 0.03f;
    public float allowedStepOffset = 0.50f;
    public int stepsUntilReevaluation = 5;

    public bool debug = false;

    protected AStar astar;

    // Path found by astar
    [SerializeField]
    protected List<Globals.Direction> path;
    [SerializeField]
    protected int tilesMoved;

    // Data about the path the monster is on
    [SerializeField]
    protected Tile startTile;
    [SerializeField]
    protected Tile currentTile;
    [SerializeField]
    protected Tile nextTile;
    [SerializeField]
    protected Tile targetTile;

    protected int stepsTaken;

    // Transition conditions
    protected bool pathNeedsReevaluation;

    void Start()
    {
        creatureTransform = creature.transform;

        astar = new AStar(tileMap);
    }

    /// <summary>
    /// Draws a line to show the path the monster will take.
    /// Debug must be on to work.
    /// </summary>
    public void OnDrawGizmos()
    {
        if (!debug) return;
        if (!Application.isPlaying) return;

        Vector2 startPoint = this.transform.position;
        for(int i = tilesMoved; i < path.Count; i++)
        {
            var v = path[i];
            var nextDirection = Globals.DirectionToVector(v);
            Gizmos.DrawRay(startPoint, nextDirection);
            startPoint += nextDirection;
        }
    }

    protected override void SetTarget(GameObject target)
    {
        this.target = target;
    }

    // ================================================
    // | States
    // ================================================

    /// <summary>
    /// Create the path from current location to the target object.
    /// Uses A Star to find the path.
    /// </summary>
    protected override void ExecuteActionFindPath()
    {
        startTile = tileMap.GetNearestTile(creatureTransform.position);
        currentTile = startTile;
        targetTile = tileMap.GetNearestTile(target.transform.position);

        // Create the path
        path = astar.FindPath(currentTile, targetTile);

        // We have not moved any tiles
        tilesMoved = 0;

        pathNeedsReevaluation = false;
    }

    protected override void ExecuteActionStartStep()
    {
        // Each step is a Move call
        stepsTaken = 0;

        nextTile = tileMap.NextTile(currentTile, path[tilesMoved]);
    }

    protected override void ExecuteActionStepping()
    {
        // Move in the direction of the next tile
        creature.Move(path[tilesMoved]);

        stepsTaken++;
    }

    protected override void ExecuteActionEvaluateStep()
    {
        Vector2 nextTilePosition = nextTile.transform.position;
        // Check how close we got to the next tile
        float stepOffset = Vector2.Distance(creatureTransform.position, nextTilePosition);

        if(stepOffset < allowedStepOffset)
        {
            // Move the creature to the center of the tile
            creatureTransform.position = Vector2.MoveTowards(creatureTransform.position, nextTilePosition, delayBetweenSteps * Time.deltaTime);
        }
        else
        {
            // TODO: Creature is too far from the tile, do something...
            pathNeedsReevaluation = true;
            return;
        }

        // We have moved a tile
        tilesMoved++;
        currentTile = nextTile;

        // The creature checks if the target has moved so the path is stale
        if(tilesMoved % stepsUntilReevaluation == 0)
        {
            // The tile the target is on now
            Tile targetCurrentTile = tileMap.GetNearestTile(target.transform.position);

            if(targetCurrentTile != targetTile)
            {
                pathNeedsReevaluation = true;
                return;
            }
        }

    }

    // ================================================
    // | Transitions
    // ================================================

    protected override bool NewPath()
    {
        return pathNeedsReevaluation;
    }

    protected override bool StepDone()
    {
        return stepsTaken >= stepAmount;
    }
}
