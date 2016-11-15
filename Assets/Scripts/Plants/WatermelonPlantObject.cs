using UnityEngine;
using System.Collections;

public class WaterMelonPlantObject : PlantGridObject
{

    public float speed;
    private float moveNum;
    private MoveableGridObject enemy;

    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;

    private Collider2D directionalCollider;

    private Animator animator;
    int directionInt; // 0 North, 1 South, 2 East, 3 West

    public MoveableGridObject seed;

    // Use this for initialization
    void Start()
    {
        // setting direction for corresponding animation
        animator = GetComponent<Animator>();
        //setDirection();
        Shooter();
    }
    void Shooter()
    {
        if (direction == Globals.Direction.North)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }

        for(int i=0; i<100; i++)
            seed.Move(direction);

    }
    // Update is called once per frame
    protected virtual void Update()
    {

        base.Update();
    }
    /**
    void setDirection()
    {
        switch (this.direction)
        {
            case Globals.Direction.North:
                southCollider.enabled = false;
                eastCollider.enabled = false;
                northCollider.enabled = true;
                westCollider.enabled = false;
                directionalCollider = northCollider;
                directionInt = 0;
                break;
            case Globals.Direction.South:
                southCollider.enabled = true;
                eastCollider.enabled = false;
                northCollider.enabled = false;
                westCollider.enabled = false;
                directionalCollider = southCollider;
                directionInt = 1;
                break;
            case Globals.Direction.East:
                southCollider.enabled = false;
                eastCollider.enabled = true;
                northCollider.enabled = false;
                westCollider.enabled = false;
                directionalCollider = eastCollider;
                directionInt = 2;
                break;
            case Globals.Direction.West:
                southCollider.enabled = false;
                eastCollider.enabled = false;
                northCollider.enabled = false;
                westCollider.enabled = true;
                directionalCollider = westCollider;
                directionInt = 3;
                break;
        }
        animator.SetInteger("Direction", directionInt); //this line animates?
    }**/
}
