using UnityEngine;
using System.Collections;

public class FireMonsterBehaviour : MonsterBehaviourAbstractFSM {

    public BehaviourModule pathFindingModule;
    public BehaviourModule primaryBehaviourModule;

    public override void Reset()
    {
    }

    // ================================================
    // | States
    // ================================================

    protected override IEnumerator ExecuteActionPathFinding()
    {
        pathFindingModule.Step();

        yield return null;
    }

    protected override IEnumerator ExecuteActionDamaged()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        primaryBehaviourModule.Step();

        yield return null;
    }

    // ================================================
    // | Transitions
    // ================================================

    protected override bool IsPathStale()
    {
        // TODO:
        return false;
    }

    protected override bool Recovered()
    {
        // TODO:
        return false;
    }

    protected override bool OnHit()
    {
        // TODO:
        return false;
    }

    protected override bool CanAct()
    {
        // TODO:
        return false;
    }
}
