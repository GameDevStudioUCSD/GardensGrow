using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveableGridObject : GridObject
{
    // Variables to set to prevent actually moving in given directions.
    public bool cantMoveLeft;
    public bool cantMoveRight;
    public bool cantMoveUp;
    public bool cantMoveDown;
    public float x, y;

    SpeedModifier baseSpeed;

    /// <summary>
    /// Direction that the object is moving/being pushed towards, even if it is not actually physically moving due to walls etc.
    /// </summary>
    protected Globals.Direction movingTowardsDirection; 

    private const float minSpeed = 0.00001f;
    public LinkedList<SpeedModifier> speedModifiers; //List of speedModifiers to speed

    protected TileMap tileMap;

    public override void Awake()
    {
        speedModifiers = new LinkedList<SpeedModifier>();
    }

    public override void Start()
    {
        base.Start();
        speedModifiers = new LinkedList<SpeedModifier>();
        baseSpeed = new SpeedModifier(Vector2.zero);
        speedModifiers.AddLast(baseSpeed);
        cantMoveLeft = false;
        cantMoveRight = false;
        cantMoveDown = false;
        cantMoveUp = false;
        tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
    }
    
    public override void Update() {
    	base.Update();

        //Applies all UncollidableStaticGridObject effects of tile currently in
        Tile t = tileMap.GetTileStandingOn(gameObject.transform.position);
        if (t != null)
        {
            t.ApplyEffects(this);
        }

        // Computes movement speed and moves if can. 
        Vector2 movement = Vector2.zero;
        foreach (SpeedModifier m in speedModifiers)
        {
            movement += m.GetSpeed();
        }

        x = movement.x;
        y = movement.y;

        float absX = Mathf.Abs(movement.x);
        float absY = Mathf.Abs(movement.y);
        if (absX > minSpeed)
        {
            if (movement.x > 0)
            {
                if (cantMoveRight)
                {
                    movement.x = 0;
                }
                if (absY > minSpeed)
                {
                    if (movement.y > 0)
                    {
                        if (cantMoveUp)
                        {
                            movement.y = 0;
                        }
                        movingTowardsDirection = Globals.Direction.Northeast;
                    }
                    else
                    {
                        if (cantMoveDown)
                        {
                            movement.y = 0;
                        }
                        movingTowardsDirection = Globals.Direction.Southeast;
                    }
                }
                else
                {
                    movingTowardsDirection = Globals.Direction.East;
                }
            }
            else
            {
                if (cantMoveLeft)
                {
                    movement.x = 0;
                }
                if (absY > minSpeed)
                {
                    if (movement.y > 0)
                    {
                        if (cantMoveUp)
                        {
                            movement.y = 0;
                        }
                        movingTowardsDirection = Globals.Direction.Northwest;
                    }
                    else
                    {
                        if (cantMoveDown)
                        {
                            movement.y = 0;
                        }
                        movingTowardsDirection = Globals.Direction.Southwest;
                    }
                }
                else
                {
                    movingTowardsDirection = Globals.Direction.West;
                }
            }
            transform.position = transform.position + new Vector3(movement.x, movement.y, 0);
        }
        else if (absY > minSpeed)
        {
            movement.x = 0;
            if (movement.y > 0)
            {
                if (cantMoveUp)
                {
                    movement.y = 0;
                }
                movingTowardsDirection = Globals.Direction.North;
            }
            else
            {
                if (cantMoveDown)
                {
                    movement.y = 0;
                }
                movingTowardsDirection = Globals.Direction.South;
            }
            transform.position = transform.position + new Vector3(movement.x, movement.y, 0);
        }
        else
        {
            movingTowardsDirection = Globals.Direction.None;
            //No actual movement
        }

        if (t != null)
        {
            t.ClearEffects(this);
        }
    }

    public void AddSpeedModifier(SpeedModifier s)
    {
        speedModifiers.AddFirst(s);
    }

    public void RemoveSpeedModifier(SpeedModifier s)
    {
        speedModifiers.Remove(s);
    }

    public void Move(Globals.Direction direction)
    {
        Vector2 v = Globals.DirectionToVector(direction);
        baseSpeed.SetSpeed(v.x / 16, v.y / 16);
    }

    public void MoveEnemy(Globals.Direction direction)
    {
        Vector2 v = Globals.DirectionToVector(direction);
        baseSpeed.SetSpeed(v.x / 16, v.y / 16);
    }
}