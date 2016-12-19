using UnityEngine;

public class EnemyGridObject : MoveableGridObject {

    private Animator animator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}

    public override bool TakeDamage(int damage)
    {
        gameObject.GetComponent<Animation>().Play("Damaged");
        return base.TakeDamage(damage);
    }

    public override void Move(Globals.Direction direction)
    {
        base.Move(direction);

        animator.SetInteger("Direction", (int)direction);
    }

    protected override bool Attack()
    {
        animator.SetTrigger("Attack");
        return base.Attack();
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Death");
    }
}
