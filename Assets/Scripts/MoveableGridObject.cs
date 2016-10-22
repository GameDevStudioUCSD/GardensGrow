using UnityEngine;
using System.Collections;

public class MoveableGridObject : RotateableGridObject {

	private const float pixelSize = 0.0625f;

	void Update() {
		if (Input.GetKey(KeyCode.UpArrow)) {
            Debug.Log("up arrow key is held down");
            Rotate(2);
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.DownArrow)) {
            Debug.Log("down arrow key is held down");
            Rotate(0);
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.RightArrow)) {
            Debug.Log("right arrow key is held down");
            Rotate(3);
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
        }
		else if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("left arrow key is held down");
            Rotate(1);
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
        }
	}
}