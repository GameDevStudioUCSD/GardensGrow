using UnityEngine;
using System.Collections;

public class WatermelonPlantObject : PlantGridObject
{
    public GameObject bullet;
    private int counter;
    public int shotDelay;
    private bool triggered = false;
    private UIController uic;

    public int changeDirectionWaitTime;
    private bool canChangeDir = true;
    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;
    private Animator animator;

    //private Collider2D directionalCollider;

    // Use this for initialization
    protected override void Start()
    {
        uic = FindObjectOfType<UIController>();
        counter = 0;
        animator = GetComponent<Animator>();
        
        //testing, doesn't know if this works yet

        southCollider.enabled = true;
        eastCollider.enabled = true;
        northCollider.enabled = true;
        westCollider.enabled = true;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        if (triggered) {
            if (!uic.paused)
            {
                if (counter > shotDelay)
                {
                    Shooter();
                    counter = 0;
                }
                counter++;
            }
        }
        base.Update();
    }
    IEnumerator changeDirectionWait()
    {
        yield return new WaitForSeconds(changeDirectionWaitTime);
        canChangeDir = true;
    }
    private void Shooter()
    {
        bullet.GetComponent<PlantProjectileObject>().dir = direction;

        audioSource.clip = attackSound;
        audioSource.Play();
        if (direction == Globals.Direction.North)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 270f);
            Instantiate(bullet, spawnPosition, spawnRotation);

            animator.SetInteger("Directions", 2);
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(bullet, spawnPosition, spawnRotation);

            animator.SetInteger("Directions", 0);
        }
        else if (direction == Globals.Direction.South)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90f);
            Instantiate(bullet, spawnPosition, spawnRotation);

            animator.SetInteger("Directions", 3);
        }
        else if(direction == Globals.Direction.East)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180f);
            Instantiate(bullet, spawnPosition, spawnRotation);

            animator.SetInteger("Directions", 1);
        }

        //animator.Stop();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || (other.CompareTag("EnemySpawner"))) {

            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if (!killable.isInvulnerable) {
                triggered = true;
                if (canChangeDir) {
                    if (other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>())) {
                        direction = Globals.Direction.South;
                        canChangeDir = false;
                        StartCoroutine(changeDirectionWait());
                    }
                    else if (other.IsTouching(northCollider.gameObject.GetComponent<BoxCollider2D>())) {
                        direction = Globals.Direction.North;
                        canChangeDir = false;
                        StartCoroutine(changeDirectionWait());
                    }
                    else if (other.IsTouching(eastCollider.gameObject.GetComponent<BoxCollider2D>())) {
                        direction = Globals.Direction.East;
                        canChangeDir = false;
                        StartCoroutine(changeDirectionWait());
                    }
                    else if (other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>())) {
                        direction = Globals.Direction.West;
                        canChangeDir = false;
                        StartCoroutine(changeDirectionWait());
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemySpawner"))
            triggered = false;
    }
}