using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGridObject : MonoBehaviour {
    private const float moveStep = 0.03125f;

    public Globals.Direction direction;
    public int damage; //units = health points
    public int delay; //units = frames
    private int delayCounter = 0;
    private UIController uic;
    //pingPong 
    public bool pingPong = false;
    public float pingPongDistance; //units = distance
    public int pingPongPause; //units = frames
    private float pingPongDistanceCounter = 0;
    private float pingPongPauseCounter = 0;
    //turbine stuff
    PlayerGridObject player;
    public PlatformTrigger southTrigger;
    public PlatformTrigger westTrigger;
    public PlatformTrigger northTrigger;
    public PlatformTrigger eastTrigger;
    private bool hasTurbine = false;
    private bool hasPlayer = false;
    private bool turbineMove = false;
    private List<GameObject> moveList = new List<GameObject>();

    // Use this for initialization
    void Start() {
        uic = FindObjectOfType<UIController>();
        player = FindObjectOfType<PlayerGridObject>();
        moveList.Add(this.gameObject);
    }
    void Update() {
        if (!uic.paused && !uic.dialogue) {
            if (pingPong) //includes miniBoss behavior
            {
                delayCounter++;
                pingPongPauseCounter--;
                if (delayCounter > delay && pingPongPauseCounter <= 0) {
                    pingPongDistanceCounter += 2 * moveStep;

                    //reverse direction
                    if (pingPongDistanceCounter > pingPongDistance) {
                        if (direction == Globals.Direction.North) direction = Globals.Direction.South;
                        else if (direction == Globals.Direction.South) direction = Globals.Direction.North;
                        else if (direction == Globals.Direction.East) direction = Globals.Direction.West;
                        else if (direction == Globals.Direction.West) direction = Globals.Direction.East;
                        pingPongDistanceCounter = 0;
                        pingPongPauseCounter = pingPongPause;
                    }

                    //move forward
                    Vector3 position = this.transform.position;
                    if (direction == Globals.Direction.North) {
                        position.y += 2 * moveStep;
                    }
                    else if (direction == Globals.Direction.South) {
                        position.y -= 2 * moveStep;
                    }
                    else if (direction == Globals.Direction.East) {
                        position.x += 2 * moveStep;
                    }
                    else if (direction == Globals.Direction.West) {
                        position.x -= 2 * moveStep;
                    }
                    transform.position = position;

                    if (hasPlayer) {
                        Vector3 playerPosition = player.transform.position;
                        if (direction == Globals.Direction.North)
                            playerPosition.y += 2 * moveStep;
                        else if (direction == Globals.Direction.South)
                            playerPosition.y -= 2 * moveStep;
                        else if (direction == Globals.Direction.East)
                            playerPosition.x += 2 * moveStep;
                        else if (direction == Globals.Direction.West)
                            playerPosition.x -= 2 * moveStep;
                        player.transform.position = playerPosition;
                    }
                    delayCounter = 0;
                }
            }
            else if (turbineMove) {
                delayCounter++;
                if (CheckStop()) {
                    turbineMove = false;
                    return;
                }
                if (delayCounter > delay) {
                    bool foundTurbine = false;
                    foreach (GameObject obj in moveList) {
                        if (!obj) {
                            moveList.Remove(obj);
                            foundTurbine = true;
                            break;
                        }
						if (obj.CompareTag("Turbine") || obj.CompareTag("Player") || 
							obj.CompareTag("Plant") || obj.CompareTag("Platform")) {
	                        if (obj.GetComponent<TurbinePlantObject>())
	                            foundTurbine = true;

	                        if (direction == Globals.Direction.East) {
	                            Vector3 position = obj.transform.position;
	                            position.x += 2 * moveStep;
	                            obj.transform.position = position;
	                        }
	                        else if (direction == Globals.Direction.West) {
	                            Vector3 position = obj.transform.position;
	                            position.x -= 2 * moveStep;
	                            obj.transform.position = position;
	                        }
	                        else if (direction == Globals.Direction.North) {
	                            Vector3 position = obj.transform.position;
	                            position.y += 2 * moveStep;
	                            obj.transform.position = position;
	                        }
	                        else if (direction == Globals.Direction.South) {
	                            Vector3 position = obj.transform.position;
	                            position.y -= 2 * moveStep;
	                            obj.transform.position = position;
	                        }
                        }
                    }
                    if (!foundTurbine) {
                        hasTurbine = false;
                    }
                    delayCounter = 0;
                }
            }
            else if (hasTurbine && CheckStart()) {
                turbineMove = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            hasPlayer = false;
            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
            player.onPlatform = false;
            moveList.Remove(col.gameObject);
        }
        if (col.gameObject.GetComponent<TurbinePlantObject>())
        {
            hasTurbine = false;
            turbineMove = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!pingPong && col.gameObject.CompareTag("Turbine")) {
            if (!turbineMove) {
                if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.West) {
                    direction = Globals.Direction.East;
                }
                else if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.East) {
                    direction = Globals.Direction.West;
                }
                else if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.North) {
                    direction = Globals.Direction.South;
                }
                else if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.South) {
                    direction = Globals.Direction.North;
                }
            }
            moveList.Add(col.gameObject);
            hasTurbine = true;
            turbineMove = true;
        }
        if (col.gameObject.CompareTag("Player")) {
            moveList.Add(col.gameObject);
            PlayerGridObject player = col.GetComponent<PlayerGridObject>();
            player.onPlatform = true;
            hasPlayer = true;
        }
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemySpawner")) {
            //there's a bug where the platform is stuck in the middle of lava if it hits a firemonster
            if (col.gameObject.GetComponent<GenericMonsterBehaviour>())
            {
                return;
            }
            KillableGridObject enemy = col.GetComponentInParent<KillableGridObject>();
            enemy.TakeDamage(damage);
        }
    }

    bool CheckStop() {
        if (direction == Globals.Direction.North && northTrigger.isTriggered) return true;
        if (direction == Globals.Direction.South && southTrigger.isTriggered) return true;
        if (direction == Globals.Direction.East && eastTrigger.isTriggered) return true;
        if (direction == Globals.Direction.West && westTrigger.isTriggered) return true;
        else return false;
    }

    bool CheckStart() {
        if (direction == Globals.Direction.North && northTrigger.isTriggered) return false;
        if (direction == Globals.Direction.South && southTrigger.isTriggered) return false;
        if (direction == Globals.Direction.East && eastTrigger.isTriggered) return false;
        if (direction == Globals.Direction.West && westTrigger.isTriggered) return false;
        else return true;
    }

    public void changeDirection(Globals.Direction directionToMoveTo) {
        for (int i = 0; i < moveList.Count; i++) {
            if (moveList[i].CompareTag("Turbine")) {
                TurbinePlantObject plant = moveList[i].GetComponent<TurbinePlantObject>();
                plant.direction = directionToMoveTo;
                plant.setDirection();
                moveList.Remove(plant.gameObject);
            }
        }
    }

    // Destroys this boat as well as the plant on it
    public void Destructor() {
        /*foreach(GameObject obj in moveList)
        {
			if (obj.CompareTag("Turbine"))
			{
				//PlantGridObject thisPlant = obj.GetComponent<PlantGridObject>();
				//moveList.Remove(obj);
				Destroy(obj);
			}
        }*/
        for (int i = 0; i < moveList.Count; i++) {
            if (moveList[i].CompareTag("Turbine") || moveList[i].CompareTag("Plant")) {
                PlantGridObject plant = moveList[i].GetComponent<PlantGridObject>();
                plant.TakeDamage(500);
            }
            if (moveList[i].CompareTag("Player")) {
                PlayerGridObject player = moveList[i].GetComponent<PlayerGridObject>();
                player.onPlatform = false;
            }
        }

        Destroy(this.gameObject);
    }
}
