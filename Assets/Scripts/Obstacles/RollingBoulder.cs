﻿using UnityEngine;
using System.Collections;

public class RollingBoulder : MoveableGridObject {

    public bool isRolling = false;
    public bool isCrumbling = false;
	private Animator animator;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        bombable = true;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	    if (isRolling) {
            Move(direction);
            Attack();
            FinishedAttack();
        }
	}

    public override void Move(Globals.Direction direction) {
        if (isRolling) {
            base.Move(direction);
        }
    }

    public void StartRolling(Globals.Direction rollDirection) {
        direction = rollDirection;
        isRolling = true;
		animator.SetInteger("Direction", (int)direction);
        animator.SetTrigger("Roll");
        GetComponent<Rigidbody2D>().WakeUp();
    }

    public override void Attack() {
        base.Attack();
        if (hitSomething) {
            animator.SetTrigger("Explode");
            isRolling = false;
            isCrumbling = true;
        }
    }

    public void PublicDeath() {
		animator.SetTrigger("Explode");
        isRolling = false;
        isCrumbling = true;
    }
    
    protected override void Die () {
    	base.Die();
	}
}
