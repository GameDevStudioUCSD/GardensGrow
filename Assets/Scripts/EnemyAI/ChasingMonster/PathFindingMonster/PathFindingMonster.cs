using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindingMonster : PathFindingMonsterAbstractFSM {

    [Header("Required Components")]
    public TileMap tileMap;
    public GameObject targetObject;

    protected AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;
    private int currentPathIndex = 0;

    public override void Start()
    {
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);

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

    protected override IEnumerator ExecuteActionPathFind()
    {
        throw new System.NotImplementedException();
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
