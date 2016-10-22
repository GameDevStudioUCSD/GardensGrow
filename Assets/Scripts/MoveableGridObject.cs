using UnityEngine;
using System.Collections;

public class MoveableGridObject : RotateableGridObject {
	public PlayerEdgeTrigger southCollider;
	public PlayerEdgeTrigger westCollider;
	public PlayerEdgeTrigger northCollider;
	public PlayerEdgeTrigger eastCollider;
	private const float pixelSize = Globals.pixelSize;

	private bool southCollision = false;
	private bool westCollision = false;
	private bool northCollision = false;
	private bool eastCollision = false;

	protected virtual void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	protected virtual void Move(int direction)
	{
		Rotate(direction);
		if (direction == 0 && !southCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == 1 && !westCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == 2 && !northCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (direction == 3 && !eastCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
	}


}