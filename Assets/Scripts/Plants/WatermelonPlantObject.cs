using UnityEngine;
using System.Collections;

public class WatermelonPlantObject : PlantGridObject
{

    public SeedProjectileObject seed;
    private int counter;
    public int shotDelay;

    public ColliderTrigger southCollider;
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

        

        if (counter > shotDelay)
        {
            Shooter();
            counter = 0;
        }
        counter++;

        base.Update();
    }

    private void Shooter()
    {
		seed.dir = direction;

        Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(seed, spawnPosition, spawnRotation);

        if (direction == Globals.Direction.North)
        {
            animator.SetInteger("Directions", 2);  
        }
        else if (direction == Globals.Direction.West)
        {
            animator.SetInteger("Directions", 0);
        }
        else if (direction == Globals.Direction.South)
        {
            animator.SetInteger("Directions", 3);
        }
        else
        {
            animator.SetInteger("Directions", 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /**
        if(other.gameObject.tag == "Enemy")
        {

            if (other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>()))
            {
                Debug.Log("hhi");
                direction = Globals.Direction.North;
            }
            else if (northCollider.isTrigger)
            {
                direction = Globals.Direction.North;
            }
            else if (eastCollider.isTrigger)
            {
                direction = Globals.Direction.East;
            }
            else
            {
                direction = Globals.Direction.West;
            }
        }**/
    }
}