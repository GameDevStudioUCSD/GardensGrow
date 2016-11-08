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

    SpeedModifier baseSpeed;

    public int count;

    /// <summary>
    /// Direction that the object is moving/being pushed towards, even if it is not actually physically moving due to walls etc.
    /// </summary>
    protected Globals.Direction movingTowardsDirection; 

    private const float minSpeed = 0.00001f;
    public LinkedList<SpeedModifier> speedModifiers; //List of speedModifiers to speed

    protected TileMap tileMap;

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        count = 0;
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

    //
    //	public PlayerEdgeTrigger southCollider;
    //	public PlayerEdgeTrigger westCollider;
    //	public PlayerEdgeTrigger northCollider;
    //	public PlayerEdgeTrigger eastCollider;
    //	private const float pixelSize = Globals.pixelSize;
    //
    //	private bool southCollision = false;
    //	private bool westCollision = false;
    //	private bool northCollision = false;
    //	private bool eastCollision = false;
    //
    //    private Animator animator;
    //
    //    public override void Start() {
    //        base.Start();
    //        animator = GetComponent<Animator>();
    //    }
    //
    //	protected virtual void Update() {
    //		base.Update();
    //	}
    //
    //	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
    //	public virtual void Move(Globals.Direction direction) {
    //		Rotate(direction);
    //        if (animator.GetInteger("Direction") != (int)direction)
    //            animator.SetInteger("Direction", (int)direction);
    //        animator.SetBool("IsWalking", true);
    //        if (direction == Globals.Direction.South && !southCollider.isTriggered) {
    //			Vector3 position = this.transform.position;
    //            position.y -= pixelSize;
    //            this.transform.position = position;
    //        }
    //		else if (direction == Globals.Direction.West && !westCollider.isTriggered) {
    //			Vector3 position = this.transform.position;
    //            position.x -= pixelSize;
    //            this.transform.position = position;
    //        }
    //		else if (direction == Globals.Direction.North && !northCollider.isTriggered) {
    //			Vector3 position = this.transform.position;
    //            position.y += pixelSize;
    //            this.transform.position = position;
    //        }
    //		else if (direction == Globals.Direction.East && !eastCollider.isTriggered) {
    //			Vector3 position = this.transform.position;
    //            position.x += pixelSize;
    //            this.transform.position = position;
    //        }
    //	}
    //    
    //    public virtual void MoveEnemy(Globals.Direction direction)
    //    {
    //        Rotate(direction);
    //        if (animator == null) animator = GetComponent<Animator>();
    //        animator.SetInteger("Direction", (int)direction);
    //        if (direction == Globals.Direction.South && !southCollider.isTriggered)
    //        {
    //            Vector3 position = this.transform.position;
    //            position.y -= pixelSize;
    //            this.transform.position = position;
    //        }
    //        else if (direction == Globals.Direction.West && !westCollider.isTriggered)
    //        {
    //            Vector3 position = this.transform.position;
    //            position.x -= pixelSize;
    //            this.transform.position = position;
    //        }
    //        else if (direction == Globals.Direction.North && !northCollider.isTriggered)
    //        {
    //            Vector3 position = this.transform.position;
    //            position.y += pixelSize;
    //            this.transform.position = position;
    //        }
    //        else if (direction == Globals.Direction.East && !eastCollider.isTriggered)
    //        {
    //            Vector3 position = this.transform.position;
    //            position.x += pixelSize;
    //            this.transform.position = position;
    //        }
    //    }
    //    protected virtual void Stop() {
    //        animator.SetBool("IsWalking", false);
    //    }
}