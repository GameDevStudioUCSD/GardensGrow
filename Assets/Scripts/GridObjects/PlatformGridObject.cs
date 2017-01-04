using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGridObject : MonoBehaviour
{
    private bool hasTurbine = false;
    private bool hasPlayer = false;
    private bool move = false;
	private Globals.Direction direction;
    //Important var for controlling speed of movement
    private int counter = 0;
    private int timeKeeper = 0;
    public int delay;
    public int distance;
    public int damage;
    private UIController uic;
    //miniBoss stuff
    public bool miniBossLvl = false;
    public int moveDistance; //change to private
    private bool goingLeft = false;
    PlayerGridObject player;

    public PlatformTrigger southCollider;
    public PlatformTrigger westCollider;
    public PlatformTrigger northCollider;
    public PlatformTrigger eastCollider;

    private List<GameObject> moveList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        uic = FindObjectOfType<UIController>();
        player = FindObjectOfType<PlayerGridObject>();
        moveList.Add(this.gameObject);
    }
    void Update()
    {
        if (!uic.paused)
        {
            if (miniBossLvl)
            {
                counter++;
                if (counter > moveDistance)
                {
                    goingLeft = !goingLeft;
                    counter = 0;
                }
                if (!goingLeft)
                {

                    Vector3 position = this.transform.position;
                    position.x += .03125f;
                    this.transform.position = position;

                    if (hasPlayer)
                    {
                        Vector3 position2 = player.transform.position;
                        position2.x += .03125f;
                        player.transform.position = position2;
                    }
                }
                else
                {
                    Vector3 position = this.transform.position;
                    position.x -= .03125f;
                    this.transform.position = position;

                    if (hasPlayer)
                    {
                        Vector3 position2 = player.transform.position;
                        position2.x -= .03125f;
                        player.transform.position = position2;
                    }
                }

            }
            if (move && miniBossLvl == false)
            {
                timeKeeper++;
                counter++;
                if (CheckStop())
                {
                    move = false;
                    return;
                }
                if (counter > delay)
                {
                    foreach (GameObject obj in moveList)
                    {
                        if (direction == Globals.Direction.East)
                        {
                            Vector3 position = obj.transform.position;
                            position.x += .03125f;
                            obj.transform.position = position;
                        }
                        else if (direction == Globals.Direction.West)
                        {
                            Vector3 position = obj.transform.position;
                            position.x -= .03125f;
                            obj.transform.position = position;
                        }
                        else if (direction == Globals.Direction.North)
                        {
                            Vector3 position = obj.transform.position;
                            position.y += .03125f;
                            obj.transform.position = position;
                        }
                        else if (direction == Globals.Direction.South)
                        {
                            Vector3 position = obj.transform.position;
                            position.y -= .03125f;
                            obj.transform.position = position;
                        }
                    }
                    counter = 0;
                }
            }
            else if (CheckStart()) move = true;
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
        if (col.gameObject.CompareTag("Turbine"))
        {
            hasTurbine = false;
            moveList.Remove(col.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.CompareTag("Turbine"))
        {
            if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.West)
            {
                direction = Globals.Direction.East;
            }
            else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.East)
            {
				direction = Globals.Direction.West;
            }
			else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.North)
            {
				direction = Globals.Direction.South;
            }
			else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.South)
            {
				direction = Globals.Direction.North;
            }
            moveList.Add(col.gameObject.transform.parent.gameObject);
            hasTurbine = true;
            move = true;
        }
		if (col.gameObject.CompareTag("Player"))
        {
            moveList.Add(col.gameObject);
			PlayerGridObject player = col.GetComponent<PlayerGridObject>();
			player.onPlatform = true;
            hasPlayer = true;
        }
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemySpawner"))
        {
            KillableGridObject enemy = col.GetComponentInParent<KillableGridObject>();
            enemy.TakeDamage(damage);
        }
    }
    
    bool CheckStop()
    {
        if (timeKeeper > distance) return true;
        if (direction == Globals.Direction.North && northCollider.isTriggered) return true;
		if (direction == Globals.Direction.South && southCollider.isTriggered) return true;
		if (direction == Globals.Direction.East  && eastCollider.isTriggered)  return true;
		if (direction == Globals.Direction.West  && westCollider.isTriggered)  return true;
        else return false;
    }

    bool CheckStart()
    {
        if (timeKeeper > distance) return false;
		if (direction == Globals.Direction.North && northCollider.isTriggered) return false;
		if (direction == Globals.Direction.South && southCollider.isTriggered) return false;
		if (direction == Globals.Direction.East  && eastCollider.isTriggered)  return false;
		if (direction == Globals.Direction.West  && westCollider.isTriggered)  return false;
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
        		GameObject plant = moveList[i];
        		Destroy(plant);
        	}
        	if (moveList[i].CompareTag("Player")) {
        		PlayerGridObject player = moveList[i].GetComponent<PlayerGridObject>();
        		player.onPlatform = false;
        	}
        }

		Destroy(this.gameObject);
    }
}
