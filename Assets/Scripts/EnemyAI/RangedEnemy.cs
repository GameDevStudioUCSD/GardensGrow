using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyGridObject {

    public GameObject projectile;
    public bool shootsIndefinately = false;
    private int counter=0;
    public int shotDelay;

    /**public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;*/


    void Start()
    {
        southCollider.enabled = true;
        eastCollider.enabled = true;
        northCollider.enabled = true;
        westCollider.enabled = true;
    }
    void Update()
    {
        if (shootsIndefinately)
        {
            if (counter > shotDelay)
            {
                Shooter();
                counter = 0;
            }
            counter++;
        }
    }
    private void Shooter()
    {
        projectile.GetComponent<RangedEnemyProjectile>().dir = direction;
        //seed.dir = direction;

        if (direction == Globals.Direction.North)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 270f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            //animator.SetInteger("Directions", 2);
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(projectile, spawnPosition, spawnRotation);
            //animator.SetInteger("Directions", 0);
        }
        else if (direction == Globals.Direction.South)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            //animator.SetInteger("Directions", 3);
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            //animator.SetInteger("Directions", 1);
        }

        //animator.Stop();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!shootsIndefinately)
        {
            if (other.CompareTag("Player"))
            {
                //Shooter() is being called, check
                if (counter > shotDelay)
                {
                    Shooter();
                    counter = 0;
                }
                counter++;
            }
        }
    }
    //code for if wants to shoot in Player direction;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!shootsIndefinately)
        {
            if (other.gameObject.tag == "Player")
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
                else if (other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>()))
                {
                    direction = Globals.Direction.West;
                }
            }
        }
    }
}
