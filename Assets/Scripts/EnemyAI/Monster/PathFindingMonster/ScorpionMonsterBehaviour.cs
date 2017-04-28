using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScorpionMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public PathFindingModule pathFindingModule;
    public ScorpionAttackModule scorpionAttackModule;

    public override void Reset() { }

    // =====================================================
    // | States
    // =====================================================

    protected override IEnumerator ExecuteActionPathFinding()
    {
        pathFindingModule.Step();

        yield return null;
    }

    // TODO: Maybe used for bombed state
    protected override IEnumerator ExecuteActionDamaged()
    {
        yield return null;
    }

    // Unused for scorpion
    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        yield return null;
    }

    // will be unused for scorpion
    protected override IEnumerator ExecuteActionDisabled()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        scorpionAttackModule.Step();

        yield return null;
    }

    // =====================================================
    // | Transitions
    // =====================================================

    protected override bool CanMove()
    {
        // Scorpion can get stuck while attacking
        return scorpionAttackModule.CanMove();
    }

    protected override bool CanAct()
    {
        return false;
    }

    protected override bool CanAttack()
    {
        AttackCollider attackCollider = GetHitColliderFromDirection(direction);

        List<KillableGridObject> killList = attackCollider.GetKillList();

        if (killList.Count > 0)
        {
            // Check if the killable is an enemy
            foreach (KillableGridObject tar in killList)
            {
                if (tar.faction != this.faction)
                    return true;
            }

            return false;
        }
        else
            return false;
    }

    // TODO: may be used for bomb state
    protected override bool Recovered()
    {
        return false;
    }

    // TODO: may be used for bomb state
    protected override bool OnHit()
    {
        return false;
    }

}
