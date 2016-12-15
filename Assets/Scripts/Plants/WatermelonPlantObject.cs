using UnityEngine;
using System.Collections;

public class WatermelonPlantObject : PlantGridObject
{

    public SeedProjectileObject seed;
    private int counter;
    public int shotDelay;

    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;
    private Animator animator;

    //private Collider2D directionalCollider;

    // Use this for initialization
    void Start()
    {
        counter = 0;
        animator = animator = GetComponent<Animator>();
        
        //testing, doesn't know if this works yet

        southCollider.enabled = true;
        eastCollider.enabled = true;
        northCollider.enabled = true;
        westCollider.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void Shooter()
    {
		seed.dir = direction;

        if (direction == Globals.Direction.North)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 270f);
            Instantiate(seed, spawnPosition, spawnRotation);

            animator.SetInteger("Directions", 2);  
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
            animator.SetInteger("Directions", 0);
        }
        else if (direction == Globals.Direction.South)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90f);
            Instantiate(seed, spawnPosition, spawnRotation);
            animator.SetInteger("Directions", 3);
        }
        else if(direction == Globals.Direction.East)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180f);
            Instantiate(seed, spawnPosition, spawnRotation);
            animator.SetInteger("Directions", 1);
        }

        //animator.Stop();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (counter > shotDelay)
            {
                Shooter();
                counter = 0;
            }
            counter++;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Enemy")
        {

            if (other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>()))
            {
                direction = Globals.Direction.South;
            }
            else if (other.IsTouching(northCollider.gameObject.GetComponent<BoxCollider2D>()))
            {
                direction = Globals.Direction.North;
            }
            else if (other.IsTouching(eastCollider.gameObject.GetComponent<BoxCollider2D>()))
            {
                direction = Globals.Direction.East;
            }
            else if(other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>()))
            {
                direction = Globals.Direction.West;
            }
        }
    }
}