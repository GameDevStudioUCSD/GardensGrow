using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerGridObject : MoveableGridObject {
	public PlantGridObject[] plants;
	public UIController canvas;
    public float tempInvincibiltySeconds;
    //check if can plant here

    bool canPlant = true;
    private float horizontalAxis;
    private float verticalAxis;
    public int knockBackPower;

    public Animator animator;
    public Animation anim;
    public bool canMove;

    public AudioClip invalidPlacement;

    //Used to determine if player should or shouldn't take damage when on a platform with lava
    public bool onPlatform;

	private GameObject dialogue;
    private bool invincible;
    private int frames = 0;
    private float time = 0;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        //Debug.Log("PLAYER HEALTH SHOULD BE " + Globals.playerHealth);
        canvas.UpdateHealth(Globals.playerHealth);  //update players health from load/Globals
      
        this.gameObject.transform.position = Globals.spawnLocation;

        anim = gameObject.GetComponent<Animation>();
        canMove = true;
        animator = GetComponent<Animator>();
        dialogue = canvas.dialogUI;
        Globals.player = this;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

        // TODO: pull up animator code for player up to here so monster can have their own
        // Get Left or Right
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        // Get Up or Down
        verticalAxis = Input.GetAxisRaw("Vertical");

        // Up
        if (canMove)
        {
            if (!isAttacking && verticalAxis > 0)
            {
                Move(Globals.Direction.North);
				Move(Globals.Direction.North);
                
                // Double movespeed
                if (horizontalAxis == 0.0f) Move(Globals.Direction.North);
            }
            // Down
            else if (!isAttacking && verticalAxis < 0)
            {
                Move(Globals.Direction.South);
				Move(Globals.Direction.South);

                if (horizontalAxis == 0.0f) Move(Globals.Direction.South);
            }

            // Left
            if (!isAttacking && horizontalAxis < 0)
            {
                Move(Globals.Direction.West);
				Move(Globals.Direction.West);

                if (verticalAxis == 0.0f) Move(Globals.Direction.West);
            }
            // Right
            else if (!isAttacking && horizontalAxis > 0)
            {
                Move(Globals.Direction.East);
				Move(Globals.Direction.East);

                if (verticalAxis == 0.0f) Move(Globals.Direction.East);
            }

            if (!isAttacking && (horizontalAxis != 0.0f || verticalAxis != 0.0f))
            {
                animator.SetBool("IsWalking", true);
                animator.SetInteger("Direction", (int)direction);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                animator.SetTrigger("Attack");
                Attack();

                //knockBack logic
                foreach (KillableGridObject target in killList)
                {
                    MoveableGridObject moveable = target.GetComponent<MoveableGridObject>();
                    if (moveable) {
                        if (!(moveable.gameObject.GetComponent<BombObject>()) && !(moveable.gameObject.GetComponent<RollingBoulder>())) {
                            for (int i = 0; i < this.gameObject.GetComponent<PlayerGridObject>().knockBackPower; i++) {
                                moveable.Move(this.gameObject.GetComponent<PlayerGridObject>().direction);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 10; ++i)
            {
                if (Input.GetKeyDown("" + i))
                    Plant(i - 1);
            }
        }
		
	}

    public override void Attack()
    {
        isAttacking = true;
        base.Attack();
    }

	protected virtual void Plant(int plantNumber) {
		// Plant animation in that direction
		// Check if there is space in front to plant
			// If there is plant
				// Instatiate new plant object 
				//  position it in the world

			// Else make failure animation

		// Start cooldown timer/reduce seed count
        // TODO: use more general form of detecting direction
        // Vector3 dirr = Globals.DirectionToVector(direction);
        // PlantGridObject newPlant = (PlantGridObject)Instantiate(plants[plantNumber], transform.position + dirr, Quaternion.identity);
        if (Globals.inventory[plantNumber] > 0){

            canPlant = true;
            foreach (KeyValuePair<Globals.PlantData, int> plant in Globals.plants)
            {

                if (plant.Key.PlantScene == Application.loadedLevelName)
                {
                    if (Mathf.Abs(plant.Key.PlantLocation.x - this.gameObject.transform.position.x) < 0.5
                     && Mathf.Abs(plant.Key.PlantLocation.y - this.gameObject.transform.position.y) < 0.5)
                    {
                        canPlant = false;
                    }
                }
                
            }
            if (canPlant == true)
            {
                //planting code
                PlantGridObject newPlant = (PlantGridObject)Instantiate(plants[plantNumber], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                newPlant.Rotate(direction);
                Globals.inventory[plantNumber]--;

                

                Globals.PlantData thisPlant = new Globals.PlantData(newPlant.transform.position, Application.loadedLevelName, newPlant.direction);
                Globals.plants.Add(thisPlant, plantNumber);

                canvas.UpdateUI();          //recheck if player can plant
            }
            else
            {
                audioSource.clip = invalidPlacement;
                audioSource.Play();
            }
		}
	}

    public override bool TakeDamage(int damage)
    {
        if (damage >= 1)
        {
            canvas.UpdateHealth(health - damage);
        }
        if (!invincible)
        {
            invincible = true;
            StartCoroutine(invicibilityWait());
            return base.TakeDamage(damage);
        }
        return base.TakeDamage(0);
    }
    IEnumerator invicibilityWait()
    {
        yield return new WaitForSeconds(tempInvincibiltySeconds);
        invincible = false;
    }
    protected virtual void LateUpdate() {
        float pixelSize = Globals.pixelSize;
        Vector3 current = this.transform.position;
        current.x = Mathf.Floor(current.x / pixelSize + 0.5f) * pixelSize;
        current.y = Mathf.Floor(current.y / pixelSize + 0.5f) * pixelSize;
        current.z = Mathf.Floor(current.z / pixelSize + 0.5f) * pixelSize;
        this.transform.position = current;
    }
    /*
    //below is code for de-planting your own plant
    void OnTriggerStay2D(Collider2D other)
    {
        if (isAttacking)
        {
            if (other.gameObject.GetComponent<PlantGridObject>() && this.gameObject.GetComponent<PlayerGridObject>())
            {
                other.gameObject.GetComponent<PlantGridObject>().TakeDamage(100);
                
                isAttacking = !isAttacking;
            }
        }
    }*/

}
