using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollidableMoveableGridObject : MoveableGridObject, Collidable
{
    private BoxCollider2D boxCollider;
    private LinkedList<GameObject> objectsInRange = new LinkedList<GameObject>();

    public override void Awake()
    {
        objectsInRange = new LinkedList<GameObject>();
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    public override void Update()
    {
        cantMoveLeft = false;
        cantMoveRight = false;
        cantMoveDown = false;
        cantMoveUp = false;

        ArrayList destroyed = new ArrayList();

        foreach (GameObject k in objectsInRange)
        {
            if (k == null)
            {
                destroyed.Add(k);
            }
            else
            {
                Vector2 otherPos = k.transform.position;
                Vector2 thisPos = transform.position;


                //If other is on right side
                if (otherPos.x > thisPos.x)
                {
                    // If other is on top side
                    if (otherPos.y > thisPos.y)
                    {
                        // If is blocking right side
                        if (otherPos.x - thisPos.x > otherPos.y - thisPos.y)
                        {
                            cantMoveRight = true;
                        }
                        // Else is blocking top side
                        else
                        {
                            cantMoveUp = true;
                        }
                    }
                    else
                    {
                        // If is blocking right side
                        if (otherPos.x - thisPos.x > thisPos.y - otherPos.y)
                        {
                            cantMoveRight = true;
                        }
                        // Else is blocking top side
                        else
                        {
                            cantMoveDown = true;
                        }
                    }
                }
                // Otherwise other is on left side
                else
                {
                    if (otherPos.y > thisPos.y)
                    {
                        if (thisPos.x - otherPos.x > otherPos.y - thisPos.y)
                        {
                            cantMoveLeft = true;
                        }
                        else
                        {
                            cantMoveUp = true;
                        }
                    }
                    else
                    {
                        if (thisPos.x - otherPos.x > thisPos.y - otherPos.y)
                        {
                            cantMoveLeft = true;
                        }
                        else
                        {
                            cantMoveDown = true;
                        }
                    }
                }
            }
        }

        foreach (GameObject d in destroyed)
        {
            objectsInRange.Remove(d);
        }

        // Moves, if can
        base.Update();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collidable other = collision.gameObject.GetComponent<Collidable>();
        if (other != null)
        {
            objectsInRange.AddLast(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //Collidable other = collision.gameObject.GetComponent<Collidable>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Collidable other = collision.gameObject.GetComponent<Collidable>();
        if (other != null)
        {
            objectsInRange.Remove(collision.gameObject);
        }
    }
}