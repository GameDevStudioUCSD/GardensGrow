using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Vision))]
public class PathFindingMonster : PathFindingMonsterAbstractFSM {

    [Header("Required Components")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Pathfinding Parameters")]
    [Tooltip("Number of move calls per step.")]
    [Range(1, 64)]
    public int moveAmount = 32;
    [Tooltip("Delay between each step.")]
    public float stepDelay = 1.0f;
    [Tooltip("The allowed distance error from the center of a tile when moving from tile to tile")]
    public float allowedOffset = 0.50f;
    [Tooltip("The number of steps the AI will take before reevaluating its path.")]
    [Range(1,100)]
    public int stepsUntilReevaluation = 5;

    public bool debug = false;

    protected Vision visionModule;

    protected AStar astarAlgorithm;

    // Path found by astar
    protected List<Globals.Direction> path;
    [SerializeField]
    protected int currentPathIndex = 0;

    // Data about the path the monster is on
    [SerializeField]
    protected Tile startTile;
    [SerializeField]
    protected Tile currentTile;
    [SerializeField]
    protected Tile nextTile;
    [SerializeField]
    protected Tile targetTile;

    private int stepsTaken = 0;
    private int stepsUntilReevaluationCounter = 0;
    private float stepToMoveDelay;

    // Transition conditions
    protected bool pathNeedsReevaluation = false;
    protected bool pathIsComplete = false;

    protected override void Start()
    {
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        visionModule = GetComponent<Vision>();

        startTile = tileMap.GetNearestTile(transform.position);
        // TODO: 
        if (startTile == null)
            Destroy(this);
        currentTile = startTile;

        stepToMoveDelay = stepDelay / (float)moveAmount;

        base.Start();
    }

    public void OnDrawGizmos()
    {
        if (!debug) return;

        if (!Application.isPlaying) return;

        Vector2 acc = this.transform.position;
        foreach(var v in path)
        {
            var v_real = Globals.DirectionToVector(v);
            Gizmos.DrawRay(acc, v_real);
            acc += v_real;
        }
    }

    // ============================================================
    // | States
    // ============================================================

    /// <summary>
    /// Take a step on the path to target.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ExecuteActionTakeStep()
    {
        stepsTaken = 0;

        nextTile = tileMap.NextTile(currentTile, path[currentPathIndex]);

        yield return null;
    }

    protected override IEnumerator ExecuteActionStepping()
    {
        Move(path[currentPathIndex]);

        stepsTaken++;

        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        Attack();

        yield return null;
    }

    protected override IEnumerator ExecuteActionEvaluateStep()
    {
        // Check if we got close to the next tile
        float offsetMagnitude = Vector2.Distance(transform.position, nextTile.transform.position);

        if (offsetMagnitude < allowedOffset)
        {
            // Move the monster to the center of the tile
            transform.position = nextTile.transform.position;
        }
        else
        {
            // TODO: if too far from the next tile do something
            pathNeedsReevaluation = true;
        }

        // Manipulate the index, current tile, etc. for next step
        currentPathIndex++;
        stepsUntilReevaluationCounter++;

        currentTile = nextTile;

        // AI has not exceeded number of steps before reevaluation
        if(stepsUntilReevaluationCounter < stepsUntilReevaluation)
        {
            pathNeedsReevaluation = false;
        }
        // AI has exceeded the number of steps before reevaluation
        else
        {
            // Check if target has moved since last time
            // Get tile of current target location
            Tile currentTileLocation = tileMap.GetNearestTile(targetObject.transform.position);
            if (currentTileLocation == targetTile)
            {
                // The target has not moved since last time
                pathNeedsReevaluation = false;
            }
            else
            {
                pathNeedsReevaluation = true;
            }
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionChaseTarget()
    {
        Move(direction);

        yield return null;
    }

    /// <summary>
    /// Create the path from current location to the target object
    /// </summary>
    protected override IEnumerator ExecuteActionPathFind()
    {
        if (targetObject)
        {
            targetTile = tileMap.GetNearestTile(targetObject.transform.position);

            // Find a path
            path = astarAlgorithm.FindPath(currentTile, targetTile);

            // We are on the first step of the path
            currentPathIndex = 0;

            stepsUntilReevaluationCounter = 0;
        }
        else
        {
            // Idle if there is nothing to target
            state = State.Idle;
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionWander()
    {
        Globals.Direction dir = (Globals.Direction)UnityEngine.Random.Range(0, 4);
        path.Add(dir);
        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        // Do nothing, the monster is disabled
        yield return null;
    }

    // ============================================================
    // | Transitions
    // ============================================================

    protected override bool ReevaluatePath()
    {
        return pathNeedsReevaluation;
    }

    protected override bool CanAttack()
    {
        // Check cooldown
        AttackCollider edgeTrigger = getHitColliderFromDirection(direction);

        List<KillableGridObject> killList = edgeTrigger.GetKillList();

        // Check if there is anything to kill
        if (killList.Count > 0)
        {
            // Check if any of the killables are an enemy
            foreach (KillableGridObject target in killList)
            {
                if (target.faction != this.faction)
                {
                    // Rotate in the direction of the target monster is attacking
                    Rotate(Globals.VectorsToDirection(transform.position, target.transform.position));
                    return true;
                }
            }
        }

        return false;
    }

    protected override bool CanSenseTarget()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override bool StepFinished()
    {
        // step is finished if we have gone moveAmount number of steps
        // and finished the step to move amount delay
        return stepsTaken >= moveAmount && TimeInState() > stepToMoveDelay;
    }

    protected override bool PathComplete()
    {
        return pathIsComplete;
    }

    public override void Reset() { }


}
