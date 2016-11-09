using UnityEngine;
using System.Collections;

public class Player : CollidableMoveableRotateableGridObject
{
    public Plant[] plants;
    public Enemy[] enemies;

    const float keyMovementSpeed = 0.0625f;

    private SpeedModifier horizontalMovementModifier;
    private SpeedModifier verticalMovementModifier;
    private Animator animator;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
        horizontalMovementModifier = new SpeedModifier(Vector2.zero);
        verticalMovementModifier = new SpeedModifier(Vector2.zero);
        speedModifiers.AddLast(horizontalMovementModifier);
        speedModifiers.AddLast(verticalMovementModifier);
        tileMap = GameObject.Find("TileMap").GetComponent<TileMap>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // Horizontal direction is used if travelling diagonally.
        Globals.Direction newDirection = Globals.Direction.None;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            verticalMovementModifier.SetSpeed(0, -keyMovementSpeed);
            newDirection = Globals.Direction.South;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            verticalMovementModifier.SetSpeed(0, keyMovementSpeed);
            newDirection = Globals.Direction.North;
        }
        else
        {
            verticalMovementModifier.SetSpeed(0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalMovementModifier.SetSpeed(-keyMovementSpeed, 0);
            newDirection = Globals.Direction.West;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalMovementModifier.SetSpeed(keyMovementSpeed, 0);
            newDirection = Globals.Direction.East;
        }
        else
        {
            horizontalMovementModifier.SetSpeed(0, 0);
        }

        if (newDirection == Globals.Direction.None)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
            Rotate(newDirection);
            animator.SetInteger("Direction", (int) newDirection);
        }

        for (int i = 1; i < 9; ++i)
        {
            if (Input.GetKeyDown("" + i))
            {
                Plant(i);
                break;
            }
        }
        if (Input.GetKeyDown("0"))
        {
            Unplant();
        }

        if (Input.GetKeyDown("9"))
        {
            SpawnEnemy();
        }
    }

    public virtual void Plant(int plantNumber)
    {
        Tile t = GetFacingTile();
        if (t != null && t.IsOpen())
        {
            Plant newPlant =
                (Plant)
                Instantiate(plants[plantNumber], new Vector3(t.transform.position.x, t.transform.position.y, 1),
                    Quaternion.identity);
            newPlant.SetDirection(direction);
        }

    }

    public virtual void Unplant()
    {
        Tile t = GetFacingTile();
        if (t != null && !t.IsOpen())
        {
            if (t.collidableStaticGridObject is Plant)
            {
                t.collidableStaticGridObject.Destroy();
            }
        }
    }

    public virtual void SpawnEnemy()
    {
        Tile t = GetFacingTile();
        if (t != null && t.IsOpen())
        {
            Instantiate(enemies[1], new Vector3(t.transform.position.x, t.transform.position.y, 1),
                    Quaternion.identity);
        }
    }

    private Tile GetFacingTile()
    {
        Vector3 pos = gameObject.transform.position;
        Tile t = null;
        switch (direction)
        {
            case Globals.Direction.East:
                t = tileMap.GetTileCenteredOn(pos.x + 0.8f, pos.y);
                break;
            case Globals.Direction.West:
                t = tileMap.GetTileCenteredOn(pos.x - 0.8f, pos.y);
                break;
            case Globals.Direction.North:
                t = tileMap.GetTileCenteredOn(pos.x, pos.y + 0.8f);
                break;
            case Globals.Direction.South:
                t = tileMap.GetTileCenteredOn(pos.x, pos.y - 0.8f);
                break;
        }
        return t;
    }
}