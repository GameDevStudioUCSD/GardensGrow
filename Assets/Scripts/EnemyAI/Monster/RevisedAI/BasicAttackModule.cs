using UnityEngine;
using System.Collections;
using System;

public class BasicAttackModule : AttackAbstractFSM {

    protected BasicAttackParameters p;

    protected bool canAttack = false;
    protected float attackTimer = 0.0f;

    public void SetParameters(BasicAttackParameters p)
    {
        this.p = p;
    }

    public void Update()
    {
        // Null guard
        if (p == null)
            return;

        // Attack cooldown timer
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > p.attackCooldown)
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
        p.creature.Attack();
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
        return !p.creature.isAttacking;
    }

    [Serializable]
    public class BasicAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown;
    }
}
