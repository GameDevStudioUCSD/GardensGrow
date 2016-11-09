using UnityEngine;
using System.Collections;

public class MoveableGridObject : KillableGridObject {

	public PlayerEdgeTrigger southCollider;
	public PlayerEdgeTrigger westCollider;
	public PlayerEdgeTrigger northCollider;
	public PlayerEdgeTrigger eastCollider;
	private const float pixelSize = Globals.pixelSize;

	private bool southCollision = false;
	private bool westCollision = false;
	private bool northCollision = false;
	private bool eastCollision = false;

    private Animator animator;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

	protected virtual void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	public virtual void Move(Globals.Direction direction) {
		Rotate(direction);
        if (animator.GetInteger("Direction") != (int)direction)
            animator.SetInteger("Direction", (int)direction);
        animator.SetBool("IsWalking", true);
        if (direction == Globals.Direction.South && !southCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.West && !westCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.North && !northCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.East && !eastCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
	}
    public virtual void MoveEnemy(Globals.Direction direction)
    {
        Rotate(direction);
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetInteger("Direction", (int)direction);
        if (direction == Globals.Direction.South && !southCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West && !westCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North && !northCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East && !eastCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
    }
    protected virtual void Stop() {
        animator.SetBool("IsWalking", false);
    }
}