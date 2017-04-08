using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attack module for wind monsters.  
/// It has the ability to pass the wind spinning status effect.
/// </summary>

// TODO: might be easier to generalize this and make any attack pass status effects

public class WindAttackModule : BasicAttackAbstractFSM {

    public WindAttackParameters parameters;

    protected bool canAttack = false;
    protected float attackTimer = 0.0f;

    public void Update()
    {
        // Attack cooldown timer
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer > parameters.attackCooldown)
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
        // Get the targets of the attack
        AttackCollider attackCollider = parameters.creature.GetHitColliderFromDirection();
        List<KillableGridObject> targets = attackCollider.GetKillList();

        if(targets.Count > 0)
        {
            // Puts attack on cooldown
            canAttack = false;
            
            // Damage each target
            foreach (KillableGridObject target in targets)
            {
                if (target.faction != parameters.creature.faction)
                {
                    target.TakeDamage(parameters.creature.damage);

                    // Apply status effect to targets
                    GameObject statusEffect = Instantiate(parameters.statusEffectPrefab);
                    statusEffect.GetComponent<StatusEffect>().ApplyEffect(target, parameters.effectDuration);
                }
            }
        }
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
        return canAttack;
    }

    [Serializable]
    public class WindAttackParameters : AttackAbstractParameters
    {
        [Range(0.0f, 60.0f)]
        public float attackCooldown;
        public GameObject statusEffectPrefab;
        [Range(0.0f, 60.0f)]
        public float effectDuration;
    }
}
