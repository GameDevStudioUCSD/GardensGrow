using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FireMonsterBehaviour : MonsterBehaviourAbstractFSM {

    [Header("Behaviour Modules")]
    public BehaviourModule pathFindingModule;
    public BehaviourModule attackModule;

    [Header("AI Parameters")]
    public GameObject mainTarget;

    private GameObject currentTarget;

    protected override void Start()
    {
        if (mainTarget) currentTarget = mainTarget;

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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    protected override bool CanAttack()
    {
        AttackCollider attackCollider = getHitColliderFromDirection(direction);

        List<KillableGridObject> killList = attackCollider.GetKillList();

        if (killList.Count > 0)
            return true;
        else
            return false;
    }
}
