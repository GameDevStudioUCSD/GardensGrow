using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyGridObject {

    public GameObject projectile;
    //public bool shootsIndefinately = false;

    /*NOTE: commented out stuff can be reused later for some projectile shooter
     * that can shoot indefinately or shoot when only detecting the player
     */

    public bool isShooter = false;
    private int counter=0;
    public int shotDelay;
    private UIController uic;
    //private int shotRangeCounter = 0;
    public GameObject laser;


    void Start()
    {
        uic = FindObjectOfType<UIController>();

        animator = this.gameObject.GetComponent<Animator>();
        southCollider.enabled = true;
        eastCollider.enabled = true;
        northCollider.enabled = true;
        westCollider.enabled = true;
    }
    void LateUpdate()
    {
        if (!uic.paused)
        {
            if (isShooter)
            {
                if (counter > shotDelay)
                {
                    Shooter();
                    counter = 0;
                }
                counter++;

                if (health <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void Shooter()
    {
        projectile.GetComponent<RangedEnemyProjectile>().dir = direction;

        if (direction == Globals.Direction.North)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 270f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            animator.SetInteger("Direction", 3);
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x-.22f, this.gameObject.transform.position.y-.1f, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(projectile, spawnPosition, spawnRotation);
            animator.SetInteger("Direction", 1);
        }
        else if (direction == Globals.Direction.South)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y-.1f, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            animator.SetInteger("Direction", 0);
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x+.23f, this.gameObject.transform.position.y-.11f, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            animator.SetInteger("Direction", 2);
        }

        //animatorator.Stop();
    }
    /*
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
    }*/
    //code for if wants to shoot in Player direction;
    void OnTriggerStay2D(Collider2D other)
    {
        if (!uic.paused) {
            if (!isShooter)
            {
                if (other.gameObject.tag == "Player")
                {

                    if (other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>()) && direction == Globals.Direction.South)
                    {
                        other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(damage);
                    }
                    else if (other.IsTouching(northCollider.gameObject.GetComponent<BoxCollider2D>()) && direction == Globals.Direction.North)
                    {
                        other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(damage);
                    }
                    else if (other.IsTouching(eastCollider.gameObject.GetComponent<BoxCollider2D>()) && direction == Globals.Direction.East)
                    {
                        other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(damage);
                    }
                    else if (other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>()) && direction == Globals.Direction.West)
                    {
                        other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(damage);
                    }
                }
            }
        }

        /**
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
        }*/
    }
}
