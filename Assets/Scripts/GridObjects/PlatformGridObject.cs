using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGridObject : MonoBehaviour
{
    private bool hasTurbine = false;
    private bool hasPlayer = false;
    private bool move = false;
    private bool goingRight;    //there's 2 bools for this incase we wanna go up and down 2
    private bool goingLeft;
    //Important var for controlling speed of movement
    private int counter=0;
    private int timeKeeper = 0;
    public int delay;
    public int distance;

    private List<GameObject> moveList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        moveList.Add(this.gameObject);
    }
    void Update()
    {
        if (move)
        {
            timeKeeper++;
            counter++;
            if (counter > delay)
            {
                foreach (GameObject obj in moveList)
                {
                    if (goingRight)
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
                    else if(goingLeft)
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
                }
                counter = 0;
            }
            if(timeKeeper > distance)
            {
                move = false;
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
                hasPlayer = true;
            }
        }
        if (!hasTurbine)
        {
            if (col.gameObject.CompareTag("Turbine"))
            {
                if (col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.West)
                {
                    goingRight = true;
                }
                else if(col.gameObject.GetComponentInParent<TurbinePlantObject>().direction == Globals.Direction.East)
                {
                    goingLeft = true;
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
        }
        if (col.gameObject.CompareTag("Turbine"))
        {
            hasTurbine = false;
        }
    }
}
