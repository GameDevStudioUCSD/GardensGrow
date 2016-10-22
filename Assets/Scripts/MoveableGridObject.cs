using UnityEngine;
using System.Collections;

public class MoveableGridObject : RotateableGridObject {

	private const float pixelSize = 0.0625f;
	public BoxCollider2D southCollider;
	public BoxCollider2D westCollider;
	public BoxCollider2D northCollider;
	public BoxCollider2D eastCollider;

	private bool southCollision = false;
	private bool westCollision = false;
	private bool northCollision = false;
	private bool eastCollision = false;

	protected virtual void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	public void Move(int direction)
	{
		Rotate(direction);
		if (direction == 0 && !southCollision) {
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == 1 && !westCollision) {
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == 2 && !northCollision) {
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (direction == 3 && !eastCollision) {
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
	}


}