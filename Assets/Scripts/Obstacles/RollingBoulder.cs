using UnityEngine;
using System.Collections;

public class RollingBoulder : MoveableGridObject {

    public bool isRolling = false;
    public bool isCrumbling = false;
	private Animator animator;

    protected new const int numDyingFrames = 51;

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

    public void startRolling(Globals.Direction rollDirection) {
        direction = rollDirection;
        isRolling = true;
		animator.SetInteger("Direction", (int)direction);
        animator.SetTrigger("Roll");
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
    }
    
    protected override void Die () {
    	base.Die();
	}
}
