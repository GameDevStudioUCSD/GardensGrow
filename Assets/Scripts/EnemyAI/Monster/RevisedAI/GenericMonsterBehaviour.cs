using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenericMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public PathFindingBehaviour pathFindingModule;
    public BasicAttackModule attackModule;

    [Header("Parameters for Modules")]
    public PathFindingBehaviour.PathFindingParameters pathFindingParameters;

    protected override void Start()
    {
        pathFindingModule.SetParameters(pathFindingParameters);

        base.Start();
    }

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

    protected override IEnumerator ExecuteActionDetect()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack()
    {
        attackModule.Step();

        yield return null;
    }

    // ================================================
    // | Transitions
    // ================================================

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

    protected override bool Detected()
    {
        // TODO:
        return false;
    }

    protected override bool CanAttack()
    {
        AttackCollider attackCollider = getHitColliderFromDirection(direction);

        List<KillableGridObject> killList = attackCollider.GetKillList();

        if (killList.Count > 0)
        {
            // Check if the killable is an enemy
            foreach(KillableGridObject tar in killList)
            {
                if (tar.faction != this.faction)
                    return true;
            }

            return false;
        }
        else
            return false;
    }
}
