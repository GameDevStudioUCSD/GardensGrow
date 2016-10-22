using UnityEngine;
using System.Collections;

public class PlayerGridObject : MoveableGridObject {

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update();
		if (Input.GetKey(KeyCode.DownArrow)) {
			Move(Globals.Direction.South);
        }
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			Move(Globals.Direction.West);
        }
		else if (Input.GetKey(KeyCode.UpArrow)) {
			Move(Globals.Direction.North);
        }
		else if (Input.GetKey(KeyCode.RightArrow)) {
			Move(Globals.Direction.East);
		}
	}
}
