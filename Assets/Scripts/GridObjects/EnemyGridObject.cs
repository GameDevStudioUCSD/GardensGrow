using UnityEngine;

public class EnemyGridObject : MoveableGridObject {

    public Animator animator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = this.GetComponent<Animator>();
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
        animator.SetInteger("Direction", (int)direction);
        base.Move(direction);
    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
        base.Attack();
    }

    /// <summary>
    /// Attack with a different animator trigger.
    /// </summary>
    /// <param name="triggerName">Trigger name for animator state switch.</param>
    public virtual void Attack(string triggerName)
    {
        animator.SetTrigger(triggerName);
        base.Attack();
    }

    public void AnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Death");
    }
}
