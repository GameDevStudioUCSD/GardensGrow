﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenericMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public PathFindingModule pathFindingModule;
    public AttackModule attackModule;
    public EnemySpawner spawner;

    public override void Reset() { }

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

    protected override IEnumerator ExecuteActionAttack()
    {
        attackModule.Step();

        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        yield return null;
    }

    // ================================================
    // | Transitions
    // ================================================

    protected override bool Recovered()
    {
        return false;
    }

    protected override bool OnHit()
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

    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        yield return null;
    }

    protected override bool CanMove()
    {
        return true;
    }

    protected override bool CanAct()
    {
        return false;
    }
}
