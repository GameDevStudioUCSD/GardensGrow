using UnityEngine;
using System.Collections;

public class PlayerGridObject : MoveableGridObject {
	public PlantGridObject[] plants;

    // Use this for initialization
    protected virtual void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update();
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			Move (Globals.Direction.South);
		} else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			Move (Globals.Direction.West);
		} else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			Move (Globals.Direction.North);
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			Move (Globals.Direction.East);
		} else {
			for (int i = 0; i < 10; ++i) {
				if (Input.GetKeyDown ("" + i))
					Plant(i);
			}

			if (!(Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.RightArrow)))
				Stop ();
		}
	}
		
	protected virtual void Plant(int plantNumber) {
		// Plant animation in that direction
		// Check if there is space in front to plant
			// If there is plant
				// Instatiate new plant object 
				//  position it in the world

			// Else make failure animation

		// Start cooldown timer/reduce seed count
        // TODO: use more general form of detecting direction
        // Vector3 dirr = Globals.DirectionToVector(direction);
        // PlantGridObject newPlant = (PlantGridObject)Instantiate(plants[plantNumber], transform.position + dirr, Quaternion.identity);
        if (Globals.inventory[plantNumber] > 0){
			switch (direction) {
				case Globals.Direction.East:
					if (!eastHitCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x + 1, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.West:
					if (!westHitCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x - 1, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.South:
					if (!southHitCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y - 1, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.North:
					if (!northHitCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y + 1, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				default:
					break;
			}
			Globals.inventory[plantNumber]--;
		}

	}
    protected virtual void LateUpdate() {
        float pixelSize = Globals.pixelSize;
        Vector3 current = this.transform.position;
        current.x = Mathf.Floor(current.x / pixelSize + 0.5f) * pixelSize;
        current.y = Mathf.Floor(current.y / pixelSize + 0.5f) * pixelSize;
        current.z = Mathf.Floor(current.z / pixelSize + 0.5f) * pixelSize;
        this.transform.position = current;
    }
}
