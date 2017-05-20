using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScorpionAttackModule : ScorpionAttackAbstractFSM {

    public ScorpionAttackParameters parameters;

    // Change this based on animator
    protected const string tailAttackTrigger = "TailAttack";
    protected const string tailChargeTrigger = "ChargeTail";
    protected const string tailStuckTrigger = "TailStuck";
    protected const string tailUnstuckTrigger = "TailUnstuck";
    protected bool tailIsCharging = false;
    protected bool tailIsStuck = false;
    protected bool tailHasHit = false;

    /// <summary>
    /// Depending on the state of the attack, the scorpion
    /// can or cannot move.
    /// 
    /// For example, it cannot move if the tail is stuck but
    /// it can move if the attack is on cooldown.
    /// </summary>
    public bool CanMove()
    {
        return !tailIsCharging && !tailIsStuck;
    }

    // =====================================================
    // | States
    // =====================================================

    protected override void ExecuteActionReady()
    {
        // Reset transition and state flags
        tailIsCharging = false;
        tailIsStuck = false;
        tailHasHit = false;
    }

    protected override void ExecuteActionChargingTail()
    {
        tailIsCharging = true;
    }

    protected override void ExecuteActionTailAttack()
    {

        // Check if the tail hit anything
        tailHasHit = parameters.creature.HitSomething();

        parameters.creature.SetAnimatorTrigger(tailAttackTrigger);
        // Get the targets of the attack
        AttackCollider attackCollider = parameters.creature.GetHitColliderFromDirection();
        List<KillableGridObject> targets = attackCollider.GetKillList();

        if (targets.Count > 0)
        {
            // Damage each target
            foreach (KillableGridObject target in targets)
            {
                if (target.faction != parameters.creature.faction)
                {
                    target.TakeDamage(parameters.creature.damage);

                    // Apply status effect to targets
                    StatusEffect.ApplyStatusEffect(parameters.creature, target, parameters.scorpionVenom);
                }
            }
        }
    }

    protected override void ExecuteActionAttackCooldown()
    {

        tailIsCharging = false;
        tailIsStuck = false;
    }

    protected override void ExecuteActionTailStuck()
    {
        tailIsStuck = true;
    }

    // =====================================================
    // | States
    // =====================================================

    protected override bool ShouldChargeTail()
    {
        // Trigger tail charging animation
        parameters.creature.SetAnimatorTrigger(tailChargeTrigger);
        return true;
    }

    protected override bool HasFinishedCooldown()
    {
        return TimeInState() > parameters.attackCooldown;
    }

    protected override bool IsTailChargeComplete()
    {
        return TimeInState() > parameters.tailChargeDuration;
    }

    protected override bool IsTailUnstuck()
    {
        if (TimeInState() > parameters.tailStuckDuration)
        {
            // Go back to walking animations
            parameters.creature.SetAnimatorTrigger(tailUnstuckTrigger);
            return true;
        }
        else
            return false;
    }

    protected override bool HasTailHit()
    {
        if(tailHasHit)
        {
            // Go back to walking animations
            parameters.creature.SetAnimatorTrigger(tailUnstuckTrigger);
        }
        else
        {
            // Trigger tail stuck animation
            parameters.creature.SetAnimatorTrigger(tailStuckTrigger);
        }

        return tailHasHit;
    }

    [Serializable]
    public class ScorpionAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown = 1.0f;
        [Range(0.0f, 60.0f)]
        public float tailChargeDuration = 2.0f;
        [Range(0.0f, 60.0f)]
        public float tailStuckDuration = 2.0f;
        public GameObject scorpionVenom;
    }
}
