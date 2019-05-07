using UnityEngine;

public class MoveableGridObject : KillableGridObject {

	public EdgeTrigger southCollider;
	public EdgeTrigger westCollider;
	public EdgeTrigger northCollider;
	public EdgeTrigger eastCollider;

	private const float pixelSize = Globals.pixelSize;

    protected override void Start() {
        base.Start();
    }

	protected override void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	public virtual void Move(Globals.Direction direction) {
        if (Globals.canvas == null || Globals.player == null) return;
        if ((!Globals.canvas.dialogue || !Globals.player != this) && !isDying) {
		    Rotate(direction);
            if (direction == Globals.Direction.South && (!southCollider || !southCollider.isTriggered)) {
			    Vector3 position = this.transform.position;
                position.y -= pixelSize;
                this.transform.position = position;
            }
		    else if (direction == Globals.Direction.West && (!westCollider || !westCollider.isTriggered)) {
			    Vector3 position = this.transform.position;
                position.x -= pixelSize;
                this.transform.position = position;
            }
		    else if (direction == Globals.Direction.North && (!northCollider || !northCollider.isTriggered)) {
			    Vector3 position = this.transform.position;
                position.y += pixelSize;
                this.transform.position = position;
            }
		    else if (direction == Globals.Direction.East && (!eastCollider || !eastCollider.isTriggered)) {
			    Vector3 position = this.transform.position;
                position.x += pixelSize;
                this.transform.position = position;
            }
        }
	}

    public AttackCollider GetHitColliderFromDirection(Globals.Direction dir)
    {
        switch(dir)
        {
            case Globals.Direction.North: return northHitCollider;
            case Globals.Direction.East: return eastHitCollider;
            case Globals.Direction.South: return southHitCollider;
            case Globals.Direction.West: return westHitCollider;
            default: return northHitCollider;
        }
    }

    public AttackCollider GetHitColliderFromDirection()
    {
        return GetHitColliderFromDirection(this.direction);
    }
}