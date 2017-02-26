using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindAttackModule : AttackAbstractFSM {

    protected WindAttackParameters p;

    protected bool canAttack = false;
    protected float attackTimer = 0.0f;
    protected Coroutine spinCoroutine;

    public void SetParameters(WindAttackParameters p)
    {
        this.p = p;
    }

    public void Update()
    {
        // Null guard
        if (p == null)
            return;

        // Attack cooldown timer
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer > p.attackCooldown)
            {
                canAttack = true;
                attackTimer = 0.0f;
            }
        }
    }

    private IEnumerator SpinTarget(KillableGridObject target)
    {
        // Disable movement for player
        if (target.tag == Globals.player_tag)
            target.gameObject.GetComponent<PlayerGridObject>().canMove = false;

        // Spin the target
        for (int i = 0; i < 4; i++)
        {
            // If the target died mid spin, exit coroutine
            if (target == null)
                yield break;

            target.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            yield return new WaitForSeconds(p.spinDuration / 4.0f);
        }

        // Reenable movement for player
        if (target.tag == Globals.player_tag)
            target.gameObject.GetComponent<PlayerGridObject>().canMove = true;
    }

    // =====================================================
    // | States
    // =====================================================

    protected override void ExecuteActionAttack()
    {
        // Get the targets of the attack
        AttackCollider attackCollider = p.creature.GetHitColliderFromDirection();
        List<KillableGridObject> targets = attackCollider.GetKillList();

        if(targets.Count > 0)
        {
            // Puts attack on cooldown
            canAttack = false;
            
            // Damage each target
            foreach (KillableGridObject target in targets)
            {
                if (target.faction != p.creature.faction)
                {
                    target.TakeDamage(p.creature.damage);

                    // Spin targets
                    StartCoroutine(SpinTarget(target));
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
        [Range(0.0f, 60.0f)]
        public float spinDuration;
    }
}
