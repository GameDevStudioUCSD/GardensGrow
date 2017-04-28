using UnityEngine;
using System.Collections;
using System;

public class BasicAttackModule : BasicAttackAbstractFSM {

    public BasicAttackParameters parameters;

    // =====================================================
    // | States
    // =====================================================

    protected override void ExecuteActionAttack()
    {
        parameters.creature.Attack();
    }

    protected override void ExecuteActionCooldown()
    {
        // TODO: do nothing, this should not exist
        // let Update do the cooldown
    }

    // =====================================================
    // | Transitions
    // =====================================================

    protected override bool AttackReady()
    {
        if (TimeInState() > parameters.attackCooldown)
            return true;
        else
            return false;
    }

    [Serializable]
    public class BasicAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown;
    }
}
