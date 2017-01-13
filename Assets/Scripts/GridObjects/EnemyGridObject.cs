using UnityEngine;

public class EnemyGridObject : MoveableGridObject {

    protected Animator animator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = this.gameObject.GetComponent<Animator>();
    }

	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}

	/* deleting this function as the killablegridobject function does the same thing
    public override bool TakeDamage(int damage)
    {
        gameObject.GetComponent<Animation>().Play("Damaged");
        return base.TakeDamage(damage);
    }
    */

    public override void Move(Globals.Direction direction)
    {
        base.Move(direction);

        animator.SetInteger("Direction", (int)direction);
    }

    protected void Attack()
    {
        animator.SetTrigger("Attack");
        base.Attack();
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Death");
    }
}
