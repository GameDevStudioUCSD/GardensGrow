using UnityEngine;
using System.Collections;
using System;

public class BasicAttackModule : AttackAbstractFSM {

    public BasicAttackParameters parameters;

    protected bool canAttack = false;
    protected float attackTimer = 0.0f;

    public void Update()
    {
        // Attack cooldown timer
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > parameters.attackCooldown)
            {
                canAttack = true;
                attackTimer = 0.0f;
            }
        }
    }

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
        // If creature is attacking, then it is not ready to attack
        return !parameters.creature.isAttacking;
    }

    [Serializable]
    public class BasicAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown;
    }
}
