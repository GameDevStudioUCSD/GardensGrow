using UnityEngine;
using System.Collections;

public class ScorpionMonsterBehaviour : MonsterBehaviourAbstractFSM {

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    // =====================================================
    // | States
    // =====================================================

    protected override IEnumerator ExecuteActionPathFinding()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionDamaged()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        throw new System.NotImplementedException();
    }

    // =====================================================
    // | Transitions
    // =====================================================

    protected override bool CanMove()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanAct()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override bool Recovered()
    {
        throw new System.NotImplementedException();
    }

    protected override bool OnHit()
    {
        throw new System.NotImplementedException();
    }
}
