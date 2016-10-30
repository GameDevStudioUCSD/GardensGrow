using UnityEngine;
using System.Collections;

public class PlayerGridObject : MoveableGridObject {

    // Use this for initialization
    protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update();
		if (Input.GetKey (KeyCode.DownArrow)) {
			Move (Globals.Direction.South);
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			Move (Globals.Direction.West);
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			Move (Globals.Direction.North);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Move (Globals.Direction.East);
		} else if (!(Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.RightArrow)))
			Stop ();
		else if (Input.GetKey (KeyCode.Z))
			Plant ();
	}
		
	protected virtual void Plant() {
		// Plant animation in that direction
		gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteWest[currentSprite];
		// Check if there is space in front to plant
			// If there is plant
				// Instatiate new plant object 
				//  position it in the world

			// Else make failure animation

		// Start cooldown timer/reduce seed count
	
	}
}
