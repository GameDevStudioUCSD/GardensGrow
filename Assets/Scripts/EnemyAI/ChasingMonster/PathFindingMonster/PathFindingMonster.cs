using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindingMonster : PathFindingMonsterAbstractFSM {

    [Header("Required Components")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Parameters")]
    [Tooltip("Number of move calls per step.")]
    [Range(0, 32)]
    public int moveAmount = 2;

    protected AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;
    private int currentPathIndex = 0;

    // Data about the path the monster is on
    private Tile currentTile;
    private Tile nextTile;
    private Tile targetTile;

    protected override void Start()
    {
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);

        currentTile = tileMap.GetNearestTile(transform.position);

        base.Start();
    }

    /// <summary>
    /// Take a step on the path to target.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ExecuteActionTakeStep()
    {
        if (path.Count == 0)
            yield return null;

        for (int i = 0; i < moveAmount; i += 1)
        {
            if (currentPathIndex < path.Count)
                Move(path[currentPathIndex]);
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionChaseTarget()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionEvaluateStep()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    protected override bool ReevaluatePath()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanSenseTarget()
    {
        throw new System.NotImplementedException();
    }

    protected override bool Continue()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
