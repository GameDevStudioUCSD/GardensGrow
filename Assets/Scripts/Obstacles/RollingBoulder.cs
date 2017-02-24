using UnityEngine;
using System.Collections;

public class RollingBoulder : MoveableGridObject {

    public bool isRolling = false;
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
            StartCoroutine(waitDieAnimation());
        }
    }

    public void PublicDeath() {
		animator.SetTrigger("Explode");
        StartCoroutine(waitDieAnimation());
    }

    public IEnumerator waitDieAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        this.Die();
    }
    protected override void Die () {
    	//animator.SetTrigger("Explode");
    	base.Die();
	}
}
