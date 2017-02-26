using UnityEngine;
using System;
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
public class PathFindingModule : PathFindingBehaviourAbstractFSM {

    protected Transform creatureTransform;

    private PathFindingParameters p;

    public bool debug = false;

    protected AStar astar = null;
    // Path found by astar
    [SerializeField]
    protected List<Globals.Direction> path;

    // Data about the path the monster is on
    protected int tilesMoved;
    protected Tile startTile;
    protected Tile currentTile;
    protected Tile nextTile;
    protected Tile targetTile;
    protected int stepsTaken;

    // Transition conditions
    protected bool pathNeedsReevaluation;
    protected bool pathIsFinished;

    public void SetParameters(PathFindingParameters p)
    {
        // TODO: need to get parameters into the script
        this.p = p;

        creatureTransform = p.creature.transform;

        if (astar == null)
            astar = new AStar(p.tileMap);
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
        for (int i = tilesMoved; i < path.Count; i++)
        {
            var v = path[i];
            var nextDirection = Globals.DirectionToVector(v);
            Gizmos.DrawRay(startPoint, nextDirection);
            startPoint += nextDirection;
        }
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
        startTile = p.tileMap.GetNearestTile(creatureTransform.position);
        currentTile = startTile;
        targetTile = p.tileMap.GetNearestTile(p.target.transform.position);

        if (startTile == targetTile)
        {
            pathIsFinished = true;
        }
        else
        {
            pathIsFinished = false;

            // Create the path
            path = astar.FindPath(currentTile, targetTile);
        }

        // We have not moved any tiles
        tilesMoved = 0;

        pathNeedsReevaluation = false;
    }

    /// <summary>
    /// Begin the step taking process.
    /// Determine which tile we want to move towards.
    /// </summary>
    protected override void ExecuteActionStartStep()
    {
        // Each step is a Move call
        stepsTaken = 0;

        nextTile = p.tileMap.NextTile(currentTile, path[tilesMoved]);
    }

    /// <summary>
    /// Take steps.
    /// Each step is a Move call in the direction
    /// of the tile we want to move towards.
    /// </summary>
    protected override void ExecuteActionStepping()
    {
        // Move in the direction of the next tile
        p.creature.Move(path[tilesMoved]);

        stepsTaken++;
    }

    protected override void ExecuteActionEvaluateStep()
    {
        Vector2 nextTilePosition = nextTile.transform.position;
        // Check how close we got to the next tile
        float stepOffset = Vector2.Distance(creatureTransform.position, nextTilePosition);

        if (stepOffset < p.allowedStepOffset)
        {
            // Move the creature to the center of the tile
            creatureTransform.position = Vector2.MoveTowards(creatureTransform.position, nextTilePosition, p.delayBetweenSteps * Time.deltaTime);
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
        if (tilesMoved % p.tilesUntilReevaluation == 0)
        {
            // The tile the target is on now
            Tile targetCurrentTile = p.tileMap.GetNearestTile(p.target.transform.position);

            if (targetCurrentTile != targetTile)
            {
                pathNeedsReevaluation = true;
                return;
            }
        }

        // If we got to the end but target still exists, reevaluate
        if (tilesMoved >= path.Count && p.target)
        {
            pathNeedsReevaluation = true;
            return;
        }
    }

    protected override void ExecuteActionPathDone()
    {
        // If target doesn't exist, do nothing
        if (p.target == null)
            return;

        startTile = p.tileMap.GetNearestTile(creatureTransform.position);
        currentTile = startTile;
        targetTile = p.tileMap.GetNearestTile(p.target.transform.position);

        // The target is somewhere else, we need to find a new path to it
        if (startTile != targetTile)
            pathIsFinished = false;
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
        return stepsTaken >= p.stepAmount;
    }

    protected override bool FinishedPath()
    {
        return !pathNeedsReevaluation && pathIsFinished;
    }

    [Serializable]
    public class PathFindingParameters
    {
        [Header("Required Components")]
        public TileMap tileMap;
        public GameObject target;
        public EnemyGridObject creature;

        [Header("Path Finding Parameters")]
        public int stepAmount = 32;
        public float delayBetweenSteps = 0.03f;
        public float allowedStepOffset = 0.50f;
        public int tilesUntilReevaluation = 5; 
    }
}