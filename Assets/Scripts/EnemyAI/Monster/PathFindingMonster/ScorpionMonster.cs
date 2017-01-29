using UnityEngine;
using System.Collections;

public class ScorpionMonster : PathFindingMonster {
    public int tailDamage = 4; //damage done by tail attack
    public int numClawAttacks = 2; //consecutive times scorpion uses claw attack before using tail attack
    public int stuckFrames = 180; //frames scorpion will be stuck after a missed tail attack
    public int unstickFrames = 3; //length of unstick animation in frames
    public AudioClip tailAttackSound; //plays for tail attack
    protected int attackCount = 0; //number of attacks performed since last tail attack
    protected int stuckCount = 0; //frames stuck

    public override void Move(Globals.Direction direction) {
        base.Move(direction);
    }

    protected override void Attack() {
        if (isAttacking)
            return;

        if (attackCount < numClawAttacks) {
            attackCount++;
            base.Attack();
        }
        else {
            attackCount = 0;

            animator.SetTrigger("Attack"); //TODO: change this to a different Tail animation
            isAttacking = true;

            if (audioSource != null) {
                audioSource.clip = tailAttackSound;
                audioSource.Play();
            }

            switch (direction) {
                case Globals.Direction.South:
                    killList = southHitCollider.GetKillList();
                    break;
                case Globals.Direction.East:
                    killList = eastHitCollider.GetKillList();
                    break;
                case Globals.Direction.North:
                    killList = northHitCollider.GetKillList();
                    break;
                case Globals.Direction.West:
                    killList = westHitCollider.GetKillList();
                    break;
            }

            /*
             * Clear all dead targets in killList.
             * This uses lambda syntax: if a KillableGridObject in
             * the list is null, then remove it.
             */
            killList.RemoveAll((KillableGridObject target) => target == null);

            // Deal damage to all targets of the enemy faction
            bool hitSomething = false;
            foreach (KillableGridObject target in killList) {
                if (target.faction != this.faction) {
                    target.TakeDamage(tailDamage);
                    hitSomething = true;
                }
            }
            // Tail gets stuck if attack hits nothing
            if (!hitSomething) {
                state = State.Disabled;
                isInvulnerable = false;
                animator.SetTrigger("GetStuck");
            }
        }
        
    }

    protected override IEnumerator ExecuteActionDisabled() {
        // Getting unstuck
        if (stuckCount < 0) {
            stuckCount--;
            if (stuckCount < -unstickFrames) {
                state = State.Idle;
                stuckCount = 0;
                isInvulnerable = true;
            }
        }
        // Still stuck
        else {
            stuckCount++;
            if (stuckCount > stuckFrames) {
                stuckCount = -1;
                animator.SetTrigger("Unstick");
            }
        }
        return null;
    }

    public override bool TakeBombDamage(int damage) {
        bool wasInvulnerable = isInvulnerable;
        isInvulnerable = false;
        bool ret = base.TakeBombDamage(damage);
        isInvulnerable = wasInvulnerable;
        return ret;
    }
}
