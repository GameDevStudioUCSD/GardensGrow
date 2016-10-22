using UnityEngine;
using System.Collections;

public class PlayerGridObject : MoveableGridObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		if (Input.GetKey(KeyCode.DownArrow)) {
			Move(0);
        }
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			Move(1);
        }
		else if (Input.GetKey(KeyCode.UpArrow)) {
			Move(2);
        }
		else if (Input.GetKey(KeyCode.RightArrow)) {
			Move(3);
		}
	}
}
