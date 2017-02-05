using UnityEngine;
using System.Collections;

public class MonsterBehaviour : MonsterBehaviourAbstractFSM {

    public PathFindingBehaviour pathFindingFSM;

    public override void Reset()
    {
    }

    // ================================================
    // | States
    // ================================================

    protected override IEnumerator ExecuteActionPathFinding()
    {
        pathFindingFSM.Step();

        yield return null;
    }

    protected override IEnumerator ExecuteActionDamaged()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        pathFindingFSM.Step();
        yield return null;
    }

    // ================================================
    // | Transitions
    // ================================================

    protected override bool IsPathStale()
    {
        return false;
    }

    protected override bool Recovered()
    {
        return false;
    }

    protected override bool OnHit()
    {
        return false;
    }
}
