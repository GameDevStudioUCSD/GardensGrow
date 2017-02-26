using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenericMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public PathFindingModule pathFindingModule;
    public BasicAttackModule attackModule;

    [Header("Behaviour Parameters")]
    public bool isDisabled;

    [Header("Parameters for Modules")]
    public PathFindingModule.PathFindingParameters pathFindingParameters;
    public BasicAttackModule.BasicAttackParameters basicAttackParameters;

    protected override void Start()
    {
        if (isDisabled)
            Disable();
        else
        {
            pathFindingModule.SetParameters(pathFindingParameters);
            attackModule.SetParameters(basicAttackParameters);
        }

        base.Start();
    }

    public override void Reset()
    {
    }

    public void Disable()
    {
        isDisabled = true;
        state = State.Disabled;
    }

    public void Enable()
    {
        isDisabled = false;
        state = State.PathFinding;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        RunFSM();
    }

    /// <summary>
    /// Called by Spawner to activate this creature
    /// </summary>
    public void SpawnStart()
    {
        pathFindingModule.SetParameters(pathFindingParameters);
        attackModule.SetParameters(basicAttackParameters);
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
        // TODO: 
        yield return null;
    }

    protected override bool CanMove()
    {
        return true;
    }

    protected override bool CanAct()
    {
        // TODO: 
        return false;
    }
}
