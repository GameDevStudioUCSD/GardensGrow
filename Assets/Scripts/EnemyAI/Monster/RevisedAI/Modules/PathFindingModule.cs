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

    public PathFindingParameters parameters;

    // Data for use with room boundary
    protected Tile spawnedTile; // Tile this creature spawned on
    protected Transform roomTransform; // Room this creature is bound to

    public bool debug = false;

    protected Transform creatureTransform = null;
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


    public void Start()
    {
        astar = new AStar(parameters.tileMap);
        creatureTransform = parameters.creature.transform;

        if (parameters.useRoomBoundary)
            SetUpRoomBoundary();

        // Try to recover from bad parameters
        if (!parameters.tileMap)
        {
            // Find tile map
            GameObject tileMapObject = GameObject.FindGameObjectWithTag(Globals.tile_map_tag);
            parameters.tileMap = tileMapObject.GetComponent<TileMap>();
        }

        if (!parameters.target)
        {
            // Use player as default target
            parameters.target = GameObject.FindGameObjectWithTag(Globals.player_tag);
        }
    }

    /// <summary>
    /// Draws a line to show the path the monster will take.
    /// Debug must be on to work.
    /// </summary>
    public void OnDrawGizmos()
    {
        if (!debug) return;
        if (!Application.isPlaying) return;

        Vector2 startPoint = transform.position;
        for (int i = tilesMoved; i < path.Count; i++)
        {
            var v = path[i];
            var nextDirection = Globals.DirectionToVector(v);
            Gizmos.DrawRay(startPoint, nextDirection);
            startPoint += nextDirection;
        }
    }

    public void RefreshPath()
    {
        pathNeedsReevaluation = true;
    }

    protected void SetUpRoomBoundary()
    {
        spawnedTile = parameters.tileMap.GetNearestTile(creatureTransform.position);

        Transform parentTransform = null;

        //null check
        if (spawnedTile)
        {
            parentTransform = spawnedTile.transform.parent;
        }
        // Find room the creature should belong to
        while(parentTransform != null)
        {
            if(parentTransform.CompareTag(Globals.room_tag))
            {
                roomTransform = parentTransform;
                break;
            }
            
            parentTransform = parentTransform.parent;
        }

        if (!roomTransform)
            Debug.LogError("Room not found, room boundary will fail.");
    }

    protected bool IsInRoomBoundary(Vector2 targetPosition)
    {
        // A room has dimensions of 17 tiles across and 11 tiles high
        // Do calculations from center tile of the room
        float halfHorizontalLength = 8.5f, halfVerticalLength = 5.5f;


        Vector2 roomCenter = this.gameObject.transform.position;
        //null check
        if (roomTransform)
        {
            roomCenter = roomTransform.position;
        }

        float boundRight = roomCenter.x + halfHorizontalLength;
        float boundLeft = roomCenter.x - halfHorizontalLength;
        float boundUp = roomCenter.y + halfVerticalLength;
        float boundDown = roomCenter.y - halfVerticalLength;

        // Check if the target is within the horizontal bounds
        if(targetPosition.x <= boundRight && targetPosition.x >= boundLeft)
        {
            // Check if the target is within the vertical bounds
            if(targetPosition.y <= boundUp && targetPosition.y >= boundDown)
            {
                return true;
            }
        }

        return false;
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
        startTile = parameters.tileMap.GetNearestTile(creatureTransform.position);
        currentTile = startTile;

        // Check room bounds
        if(parameters.useRoomBoundary)
        {
            if(IsInRoomBoundary(parameters.target.transform.position))
            {
                // Go to target like normal
                targetTile = parameters.tileMap.GetNearestTile(parameters.target.transform.position);
            }
            else
            {
                // Target the spawned tile
                targetTile = spawnedTile;
            }
        }


        if (!startTile || !targetTile || startTile == targetTile)
        {
            pathIsFinished = true;
        }
        else
        {
            pathIsFinished = false;

            // Create the path
            path = astar.FindPath(currentTile, targetTile);
        }

        if (path.Count == 0) pathIsFinished = true;

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

        nextTile = parameters.tileMap.NextTile(currentTile, path[tilesMoved]);
    }

    /// <summary>
    /// Take steps.
    /// Each step is a Move call in the direction
    /// of the tile we want to move towards.
    /// </summary>
    protected override void ExecuteActionStepping()
    {
        // Move in the direction of the next tile
        parameters.creature.Move(path[tilesMoved]);

        stepsTaken++;
    }

    protected override void ExecuteActionEvaluateStep()
    {
        Vector2 nextTilePosition = nextTile.transform.position;
        // Check how close we got to the next tile
        float stepOffset = Vector2.Distance(creatureTransform.position, nextTilePosition);

        if (stepOffset < parameters.allowedStepOffset)
        {
            // Move the creature to the center of the tile
            creatureTransform.position = Vector2.MoveTowards(creatureTransform.position, nextTilePosition, parameters.delayBetweenSteps * Time.deltaTime);
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
        if (tilesMoved % parameters.tilesUntilReevaluation == 0)
        {
            // The tile the target is on now
            Tile targetCurrentTile = parameters.tileMap.GetNearestTile(parameters.target.transform.position);

            if (targetCurrentTile != targetTile)
            {
                pathNeedsReevaluation = true;
                return;
            }
        }

        // If we got to the end but target still exists, reevaluate
        if (tilesMoved >= path.Count && parameters.target)
        {
            pathNeedsReevaluation = true;
            return;
        }
    }

    protected override void ExecuteActionPathDone()
    {
        // If target doesn't exist, do nothing
        if (parameters.target == null)
            return;

        startTile = parameters.tileMap.GetNearestTile(creatureTransform.position);
        currentTile = startTile;
        targetTile = parameters.tileMap.GetNearestTile(parameters.target.transform.position);

        // The target is somewhere else, we need to find a new path to it
        if (startTile && targetTile && startTile != targetTile)
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
        return stepsTaken >= parameters.stepAmount;
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
        // Keep monsters from pathing out of the room
        // Monsters will return back to spawn area if they try to leave
        public bool useRoomBoundary;
    }
}