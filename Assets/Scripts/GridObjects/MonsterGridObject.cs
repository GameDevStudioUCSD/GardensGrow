using UnityEngine;

public class MonsterGridObject : MoveableGridObject {

    private Animator animator;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        animator = GetComponent<Animator>();
	}
	
    public override void Move(Globals.Direction direction) {
        base.Move(direction);

        animator.SetInteger("Direction", (int)direction);
    }

    protected override void Attack() {
        animator.SetTrigger("Attack");
        base.Attack();
    }

    protected override void Die() {
        base.Die();
        animator.SetTrigger("Death");
    }
}
