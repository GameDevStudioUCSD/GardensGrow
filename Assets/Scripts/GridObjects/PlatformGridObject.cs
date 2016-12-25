using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGridObject : MonoBehaviour
{
    private bool hasTurbine = false;
    private bool hasPlayer = false;
    private bool move = false;
    private bool goingEast;    //there's 2 bools for this incase we wanna go up and down 2
    private bool goingWest;
    private bool goingNorth;
    private bool goingSouth;
    //Important var for controlling speed of movement
    private int counter = 0;
    private int timeKeeper = 0;
    public int touchingObjects = 0;
    public int delay;
    public int distance;
    public int damage;

    public PlatformTrigger southCollider;
    public PlatformTrigger westCollider;
    public PlatformTrigger northCollider;
    public PlatformTrigger eastCollider;

    private List<GameObject> moveList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        moveList.Add(this.gameObject);
    }
    void Update()
    {
        if (touchingObjects == 0) Destroy(gameObject);
        if (move)
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
                    if (goingEast)
                    {
                        if (obj.gameObject.CompareTag("Turbine"))
                        {
                            Vector3 position = obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position;
                            position.x +=.03125f;
                            obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position = position;
                        }
                        else
                        {
                            Vector3 position = obj.transform.position;
                            position.x +=.03125f;
                            obj.transform.position = position;
                        }
                    }
                    else if(goingWest)
                    {
                        if (obj.gameObject.CompareTag("Turbine"))
                        {
                            Vector3 position = obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position;
                            position.x -=.03125f;
                            obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position = position;
                        }
                        else
                        {
                            Vector3 position = obj.transform.position;
                            position.x -= .03125f;
                            obj.transform.position = position;
                        }
                    }
					else if(goingNorth)
                    {
                        if (obj.gameObject.CompareTag("Turbine"))
                        {
                            Vector3 position = obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position;
                            position.y +=.03125f;
                            obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position = position;
                        }
                        else
                        {
                            Vector3 position = obj.transform.position;
                            position.y += .03125f;
                            obj.transform.position = position;
                        }
                    }
					else if(goingSouth)
                    {
                        if (obj.gameObject.CompareTag("Turbine"))
                        {
                            Vector3 position = obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position;
                            position.y -=.03125f;
                            obj.gameObject.GetComponentInParent<TurbinePlantObject>().transform.position = position;
                        }
                        else
                        {
                            Vector3 position = obj.transform.position;
                            position.y -= .03125f;
                            obj.transform.position = position;
                        }
                    }
                }
                counter = 0;
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (!hasPlayer)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                moveList.Add(col.gameObject);
				PlayerGridObject player = col.GetComponent<PlayerGridObject>();
				player.onPlatform = true;
                hasPlayer = true;
            }
        }
        if (!hasTurbine)
        {
            if (col.gameObject.CompareTag("Turbine"))
            {
                if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.West)
                {
                    goingEast = true;
                }
                else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.East)
                {
                    goingWest = true;
                }
				else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.North)
                {
                    goingSouth = true;
                }
				else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.South)
                {
                    goingNorth = true;
                }
                moveList.Add(col.gameObject);
                hasTurbine = true;
                move = true;
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
        if (col.gameObject.CompareTag("Turbine"))
        {
            hasTurbine = false;
        }
        if (!col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Turbine") && !col.gameObject.CompareTag("Untagged"))
            touchingObjects--;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.CompareTag("Enemy"))
		{
			KillableGridObject enemy = col.GetComponentInParent<KillableGridObject>();
			enemy.TakeDamage(damage);
        }
        if (!col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Turbine") && !col.gameObject.CompareTag("Untagged"))
            touchingObjects++;
    }
    
    bool CheckStop()
    {
        if (timeKeeper > distance) return true;
        if (goingNorth && northCollider.isTriggered) return true;
        if (goingSouth && southCollider.isTriggered) return true;
        if (goingEast  && eastCollider.isTriggered)  return true;
        if (goingWest  && westCollider.isTriggered)  return true;
        else return false;
    }
}
