using UnityEngine;
using System.Collections;

public class Player : CollidableMoveableRotateableGridObject
{
    public Plant[] plants;

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
            animator.SetInteger("Direction", (int)newDirection);
        }

        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown("" + i))
            {
                Plant(i);
                break;
            }
        }
    }

    protected virtual void Plant(int plantNumber)
    {
        
        // Plant animation in that direction
        // Check if there is space in front to plant
        // If there is plant
        // Instatiate new plant object 
        //  position it in the world

        // Else make failure animation

        // Start cooldown timer/reduce seed count
        // TODO: use more general form of detecting direction
        // Vector3 dirr = Globals.DirectionToVector(direction);
        // Plant newPlant = (Plant)Instantiate(plants[plantNumber], transform.position + dirr, Quaternion.identity);

        Vector3 gridCoordinates = gameObject.transform.position;//tileMap.GetGridCoordinates(gameObject.transform.position);

        Tile t = null;
        Plant newPlant;
        switch (direction)
        {
            case Globals.Direction.East:
                t = tileMap.GetTileCenteredOn(gridCoordinates.x + 1, gridCoordinates.y);
                break;
            case Globals.Direction.West:
                t = tileMap.GetTileCenteredOn(gridCoordinates.x - 1, gridCoordinates.y);
                break;
            case Globals.Direction.North:
                t = tileMap.GetTileCenteredOn(gridCoordinates.x, gridCoordinates.y+1);
                break;
            case Globals.Direction.South:
                t = tileMap.GetTileCenteredOn(gridCoordinates.x, gridCoordinates.y-1);
                break;
        }

        if (t != null && t.IsOpen())
        {
            newPlant = (Plant)Instantiate(plants[plantNumber], new Vector3(t.transform.position.x, t.transform.position.y, 1), Quaternion.identity);
            newPlant.SetDirection(direction);
        }

        //        switch (direction) {
        //		case Globals.Direction.East:
        //			if (!eastHitCollider.isTriggered) {
        //				Plant newPlant = (Plant)Instantiate (plants[plantNumber], new Vector3 (transform.position.x + 1, transform.position.y, 0), Quaternion.identity);
        //				newPlant.Rotate(direction);
        //			}
        //			break;
        //		case Globals.Direction.West:
        //			if (!westHitCollider.isTriggered) {
        //				Plant newPlant = (Plant)Instantiate (plants[plantNumber], new Vector3 (transform.position.x - 1, transform.position.y, 0), Quaternion.identity);
        //				newPlant.Rotate(direction);
        //			}
        //			break;
        //		case Globals.Direction.South:
        //			if (!southHitCollider.isTriggered) {
        //				Plant newPlant = (Plant)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y - 1, 0), Quaternion.identity);
        //				newPlant.Rotate(direction);
        //			}
        //			break;
        //		case Globals.Direction.North:
        //			if (!northHitCollider.isTriggered) {
        //				Plant newPlant = (Plant)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y + 1, 0), Quaternion.identity);
        //				newPlant.Rotate(direction);
        //			}
        //			break;
        //		default:
        //			break;
        //		}
    }

    protected virtual void LateUpdate()
    {
//        float pixelSize = Globals.pixelSize;
//        Vector3 current = this.transform.position;
//        current.x = Mathf.Floor(current.x/pixelSize + 0.5f)*pixelSize;
//        current.y = Mathf.Floor(current.y/pixelSize + 0.5f)*pixelSize;
//        current.z = Mathf.Floor(current.z/pixelSize + 0.5f)*pixelSize;
//        this.transform.position = current;
    }
}