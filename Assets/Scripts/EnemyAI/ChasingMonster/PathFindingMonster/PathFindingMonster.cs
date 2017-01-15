using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vision))]
public class PathFindingMonster : PathFindingMonsterAbstractFSM {

    [Header("Required Components")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Parameters")]
    [Tooltip("Number of move calls per step.")]
    [Range(0, 32)]
    public int moveAmount = 2;
    [Tooltip("Delay between each step.")]
    public float stepDelay = 1.0f;
    [Tooltip("Delay between attacks.")]
    public float attackDelay = 1.0f;
    [Tooltip("The allowed distance error from the center of a tile when moving from tile to tile")]
    public float allowedOffset = 0.80f;

    protected Vision visionModule;

    protected AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;
    private int currentPathIndex = 0;

    // Data about the path the monster is on
    private Tile currentTile;
    private Tile nextTile;
    private Tile targetTile;

    // Transition conditions
    private bool pathNeedsReevaluation = false;
    private bool attackOnCooldown = false;

    protected override void Start()
    {
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        visionModule = GetComponent<Vision>();

        currentTile = tileMap.GetNearestTile(transform.position);

        base.Start();
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
        if (path.Count == 0)
            yield return null;

        nextTile = tileMap.NextTile(currentTile, path[currentPathIndex]);

        for (int i = 0; i < moveAmount; i += 1)
        {
            Move(path[currentPathIndex]);
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        if(!attackOnCooldown)
        {
            attackOnCooldown = true;
            Attack();
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionChaseTarget()
    {
        for(int i = 0; i < moveAmount; i++)
        {
            Move(direction);
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionEvaluateStep()
    {
        // Check if we got close to the next tile
        float offsetMagnitude = Vector2.Distance(transform.position, nextTile.transform.position);

        if(offsetMagnitude < allowedOffset)
        {
            // Move the monster to the center of the tile
            transform.position = nextTile.transform.position;
        }
        else
        {
            // TODO: if too far from the next tile do something
        }

        // Manipulate the index, current tile, etc. for next step
        currentPathIndex++;
        currentTile = nextTile;

        pathNeedsReevaluation = false;

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
        }
        else
        {
            // Idle if there is nothing to target
            state = State.Idle;
        }

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
        if(TimeInState() > attackDelay)
        {
            attackOnCooldown = false;

            AttackCollider edgeTrigger = getHitColliderFromDirection(direction);

            List<KillableGridObject> killList = edgeTrigger.GetKillList();

            // Check if there is anything to kill
            if (killList.Count > 0)
            {
                // Check if any of the killables are an enemy
                foreach (KillableGridObject target in killList)
                {
                    if (target.faction != this.faction)
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

    protected override bool Continue()
    {
        return !pathNeedsReevaluation && TimeInState() > stepDelay;
    }

    public override void Reset() { }

}
