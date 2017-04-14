using UnityEngine;
using System.Collections;
using System;

public class ScorpionAttackModule : ScorpionAttackAbstractFSM {

    public ScorpionAttackParameters parameters;

    protected int clawAttacksUsed = 0;
    // Change this based on animator
    protected string tailAttackTrigger = "Attack";
    // Change this based on animator
    protected string clawAttackTrigger = "Attack";
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

    protected override void ExecuteActionClawAttack()
    {
        clawAttacksUsed++;
        parameters.creature.Attack(clawAttackTrigger);
    }

    protected override void ExecuteActionChargingTail()
    {
        tailIsCharging = true;
    }

    protected override void ExecuteActionTailAttack()
    {
        // Completing an attack with the tail resets the claw attack
        clawAttacksUsed = 0;

        parameters.creature.Attack(tailAttackTrigger);

        // Check if the tail hit anything
        tailHasHit = parameters.creature.HitSomething();
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

    protected override bool ShouldTailAttack()
    {
        return clawAttacksUsed >= parameters.numClawAttacksBeforeTail;
    }

    protected override bool HasFinishedCooldown()
    {
        if (TimeInState() > parameters.attackCooldown)
            return true;
        else
            return false;
    }

    protected override bool IsTailChargeComplete()
    {
        if (TimeInState() > parameters.tailChargeDuration)
            return true;
        else
            return false;
    }

    protected override bool IsTailUnstuck()
    {
        if (TimeInState() > parameters.tailStuckDuration)
            return true;
        else
            return false;
    }

    protected override bool HasTailHit()
    {
        return tailHasHit;
    }

    protected override bool ShouldClawAttack()
    {
        return clawAttacksUsed < parameters.numClawAttacksBeforeTail;
    }

    [Serializable]
    public class ScorpionAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown;
        [Range(0, 99)]
        [Tooltip("How many times the claw attack should be used before the tail attack.")]
        public int numClawAttacksBeforeTail = 2;
        [Range(0.0f, 60.0f)]
        public float tailChargeDuration;
        [Range(0.0f, 60.0f)]
        public float tailStuckDuration;
    }
}
