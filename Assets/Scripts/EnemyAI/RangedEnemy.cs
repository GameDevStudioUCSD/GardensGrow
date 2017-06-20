using UnityEngine;
using System.Collections;

public class RangedEnemy : EnemyGridObject {

    public GameObject projectile;
    //public bool shootsIndefinitely = false;

    /*NOTE: commented out stuff can be reused later for some projectile shooter
     * that can shoot indefinitely or shoot when only detecting the player
     */

    public bool isShooter = false;
    private int counter=0;
    public int shotDelay;
    private UIController uic;
    //private int shotRangeCounter = 0;
    public GameObject laser;

    //for save state
    private bool destroyed = false;
    private float x;
    private float y;
    private float z;

    protected override void Start()
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
        if (!uic.paused && !uic.dialogue)
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
    void OnEnable()
    {
        x = this.gameObject.transform.position.x;
        y = this.gameObject.transform.position.y;
        z = this.gameObject.transform.position.z;

        destroyed = PlayerPrefsX.GetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
            + "pos x" + x + "pos y" + y + "pos z" + z + "rangedenemy");

        if (destroyed)
        {
            gameObject.SetActive(false);
        }
        /*else
        {
            gameObject.SetActive(true);
        }*/

    }
    private void OnDestroy()
    {
        destroyed = true;
    }
    void OnDisable()
    {

        PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                  + "pos x" + x + "pos y" + y + "pos z" + z + "rangedenemy", destroyed); //TODO: put false into here, and save before building the game

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
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x-.22f, this.gameObject.transform.position.y, 0.0f);
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
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x+.23f, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180f);
            Instantiate(projectile, spawnPosition, spawnRotation);
            animator.SetInteger("Direction", 2);
        }

        //animatorator.Stop();
    }
    //code for if wants to shoot in Player direction;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!uic.paused && !uic.dialogue) {
            if (!isShooter)
            {
                if (other.gameObject.tag == "Player")
                {
					PlayerGridObject player = other.GetComponent<PlayerGridObject>();
                    if (direction == Globals.Direction.South && other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>()))
                    {
						player.TakeDamage(damage);
						player.gameObject.transform.position = Globals.spawnLocation;
                    }
                    else if (direction == Globals.Direction.North && other.IsTouching(northCollider.gameObject.GetComponent<BoxCollider2D>()))
                    {
						player.TakeDamage(damage);
						player.gameObject.transform.position = Globals.spawnLocation;
                    }
                    else if (direction == Globals.Direction.East && other.IsTouching(eastCollider.gameObject.GetComponent<BoxCollider2D>()))
                    {
						player.TakeDamage(damage);
						player.gameObject.transform.position = Globals.spawnLocation;
                    }
                    else if (direction == Globals.Direction.West && other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>()))
                    {
						player.TakeDamage(damage);
						player.gameObject.transform.position = Globals.spawnLocation;
                    }
                }
            }
        }
    }
}
