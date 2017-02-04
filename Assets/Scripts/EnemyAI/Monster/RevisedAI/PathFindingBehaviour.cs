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
    public GameObject targetObject;

    [Header("Path Finding Parameters")]
    public int stepAmount = 32;
    public float delayBetweenSteps = 0.03f;
    public float allowedStepError = 0.50f;
    public int stepsUntilReevaluation = 5;

    public bool debug = false;

    protected AStar astar;

    // Path found by astar
    [SerializeField]
    private List<Globals.Direction> path;
    [SerializeField]
    private int currentPathIndex = 0;

    // Data about the path the monster is on
    [SerializeField]
    private Tile startTile;
    [SerializeField]
    private Tile currentTile;
    [SerializeField]
    private Tile nextTile;
    [SerializeField]
    private Tile targetTile;

    private int stepsTaken = 0;

    protected override void Start()
    {
        path = new List<Globals.Direction>();
        astar = new AStar(tileMap);

        startTile = tileMap.GetNearestTile(transform.position);
        currentTile = startTile;
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
        for(int i = currentPathIndex; i < path.Count; i++)
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

    protected override void ExecuteActionFindPath()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteActionStartStep()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteActionStepping()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteActionEvaluateStep()
    {
        throw new System.NotImplementedException();
    }

    // ================================================
    // | Transitions
    // ================================================

    protected override bool NewPath()
    {
        throw new System.NotImplementedException();
    }

    protected override bool StepDone()
    {
        throw new System.NotImplementedException();
    }
}
