using UnityEngine;

public class RotateableGridObject : GridObject {

	public Globals.Direction direction;

	//public BoxCollider2D face;
	
	// Use this for initialization
	protected virtual void Start () {
		direction = 0;
	}

	protected virtual void Update() {
	}

	// Changes direction and direction sprite
	public void Rotate(Globals.Direction facing)
	{
		direction = facing;
	}
}
