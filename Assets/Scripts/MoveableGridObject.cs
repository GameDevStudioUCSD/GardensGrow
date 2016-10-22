using UnityEngine;
using System.Collections;

public class MoveableGridObject : GridObject {

	private const float pixelSize = Globals.pixelSize;

	void Update() {

		if (Input.GetKey(KeyCode.UpArrow)) {
            Debug.Log("up arrow key is held down");
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.DownArrow)) {
            Debug.Log("down arrow key is held down");
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.RightArrow)) {
            Debug.Log("right arrow key is held down");
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("left arrow key is held down");
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
	}
}