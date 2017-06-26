using UnityEngine;
using System.Collections;

public class RoomRotater : MonoBehaviour {

	public GameObject NorthRoom;
	public GameObject NorthEastRoom;
	public GameObject EastRoom;
	public GameObject SouthEastRoom;
	public GameObject SouthRoom;
	public GameObject SouthWestRoom;
	public GameObject WestRoom;
	public GameObject NorthWestRoom;

	GameObject tempRoom1;
	GameObject tempRoom2;

	private Vector3 moveRight = new Vector3(14, 0, 0);
	private Vector3 moveDown = new Vector3(0, -10, 0);
	private Vector3 moveLeft = new Vector3(-14, 0, 0);
	private Vector3 moveUp = new Vector3(0, 10, 0);

	//Algorithm is basically a rotate clockwise, each room should be given a new name
	public void Toggle () {
		tempRoom1 = NorthEastRoom;
		NorthEastRoom = NorthRoom;
		NorthEastRoom.transform.position += moveRight;

		tempRoom2 = tempRoom1;
		tempRoom1 = EastRoom;
		EastRoom = tempRoom2;
		EastRoom.transform.position += moveDown;

		tempRoom2 = tempRoom1;
		tempRoom1 = SouthEastRoom;
		SouthEastRoom = tempRoom2;
		SouthEastRoom.transform.position += moveDown;

		tempRoom2 = tempRoom1;
		tempRoom1 = SouthRoom;
		SouthRoom = tempRoom2;
		SouthRoom.transform.position += moveLeft;

		tempRoom2 = tempRoom1;
		tempRoom1 = SouthWestRoom;
		SouthWestRoom = tempRoom2;
		SouthWestRoom.transform.position += moveLeft;

		tempRoom2 = tempRoom1;
		tempRoom1 = WestRoom;
		WestRoom = tempRoom2;
		WestRoom.transform.position += moveUp;

		tempRoom2 = tempRoom1;
		tempRoom1 = NorthWestRoom;
		NorthWestRoom = tempRoom2;
		NorthWestRoom.transform.position += moveUp;

		tempRoom2 = tempRoom1;
		NorthRoom = tempRoom2;
		NorthRoom.transform.position += moveRight;

		foreach (PlantGridObject plant in FindObjectsOfType<PlantGridObject>()) {

			BoomerangPlantObject bPlant = plant as BoomerangPlantObject;

			if (bPlant != null) {
				bPlant.RemoveSelfFromRoom();
			}

			if (
				Mathf.Abs(plant.gameObject.transform.position.x - NorthEastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - NorthEastRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveDown;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - EastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - EastRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveDown;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - SouthEastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - SouthEastRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveLeft;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - SouthRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - SouthRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveLeft;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - SouthWestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - SouthWestRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveUp;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - WestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - WestRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveUp;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - NorthWestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - NorthWestRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveRight;
			}
			else if (
				Mathf.Abs(plant.gameObject.transform.position.x - NorthRoom.transform.position.x) <= 7 &&
				Mathf.Abs(plant.gameObject.transform.position.y - NorthRoom.transform.position.y) <= 5
			) {
				plant.gameObject.transform.position += moveRight;
			}

			if (bPlant != null) {
				bPlant.AddSelfToRoom();
			}
		}

		foreach (EnemyGridObject enemy in FindObjectsOfType<EnemyGridObject>()) {
			if (
				Mathf.Abs(enemy.gameObject.transform.position.x - NorthEastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - NorthEastRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveDown;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - EastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - EastRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveDown;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - SouthEastRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - SouthEastRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveLeft;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - SouthRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - SouthRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveLeft;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - SouthWestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - SouthWestRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveUp;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - WestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - WestRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveUp;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - NorthWestRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - NorthWestRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveRight;
			}
			else if (
				Mathf.Abs(enemy.gameObject.transform.position.x - NorthRoom.transform.position.x) <= 7 &&
				Mathf.Abs(enemy.gameObject.transform.position.y - NorthRoom.transform.position.y) <= 5
			) {
				enemy.gameObject.transform.position += moveRight;
			}
		}

        foreach (Boomerang b in FindObjectsOfType<Boomerang>()) {
			b.UpdateRoomId();
		}
	}
}
