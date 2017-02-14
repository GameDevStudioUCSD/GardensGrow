using UnityEngine;
using System.Collections;
using System;

public class BasicAttackModule : BasicAttackAbstractFSM {

    [Header("Attacking Components")]
    public EnemyGridObject creature;

    // =====================================================
    // | States
    // =====================================================

    protected override void ExecuteActionAttack()
    {
        creature.Attack();
    }

    protected override void ExecuteActionCooldown()
    {
        // Do nothing
    }

    // =====================================================
    // | Transitions
    // =====================================================

    protected override bool AttackReady()
    {
        // If creature is attacking, then it is not ready to attack
        return !creature.isAttacking;
    }
}
