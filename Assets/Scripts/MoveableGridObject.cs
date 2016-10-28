using UnityEngine;
using System.Collections;

public class MoveableGridObject : RotateableGridObject {
//<<<<<<< HEAD

	//private const float pixelSize = 0.0625f;
//=======
//>>>>>>> 86891dc6aefd15a877787e50b55619165a8aeec5
	public BoxCollider2D southCollider;
	public BoxCollider2D westCollider;
	public BoxCollider2D northCollider;
	public BoxCollider2D eastCollider;
	private const float pixelSize = Globals.pixelSize;

	private bool southCollision = false;
	private bool westCollision = false;
	private bool northCollision = false;
	private bool eastCollision = false;

	protected virtual void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	public void Move(Globals.Direction direction)
	{
		Rotate(direction);
		if (direction == Globals.Direction.South && !southCollision) {
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.West && !westCollision) {
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.North && !northCollision) {
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (direction == Globals.Direction.East && !eastCollision) {
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("ENTERED COLLIDER");
	}


}