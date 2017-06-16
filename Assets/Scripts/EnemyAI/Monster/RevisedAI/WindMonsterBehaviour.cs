using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WindMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public PathFindingModule pathFindingModule;
    public WindAttackModule attackModule;
    public SummoningModule summonModule;
    public EnemySpawner spawner;

    public override void Reset()
    {
    }

    public override void StartAI()
    {
        // Set up tilemap and targetting for any summons
        summonModule.parameters.tileMap = pathFindingModule.parameters.tileMap;
        summonModule.parameters.target = pathFindingModule.parameters.target;

        base.StartAI();
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

    protected override IEnumerator ExecuteActionAttack()
    {
        attackModule.Step();

        yield return null;
    }

    // Summon
    protected override IEnumerator ExecuteActionPrimaryBehaviour()
    {
        summonModule.Summon();

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
        // TODO:
        return false;
    }

    protected override bool OnHit()
    {
        // TODO:
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



    protected override bool CanMove()
    {
        return true;
    }

    protected override bool CanAct()
    {
        return summonModule.CanSummon();
    }
}
