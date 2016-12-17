using UnityEngine;

public class PlayerGridObject : MoveableGridObject {
	public PlantGridObject[] plants;
	public UIController canvas;

    private float horizontalAxis;
    private float verticalAxis;
        
    private Animator animator;
    private Animation animation;
    public bool canMove;

    // Use this for initialization
    protected virtual void Start () {
        base.Start();
        animation = gameObject.GetComponent<Animation>();
        canMove = true;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
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
                // Double movespeed
                if (horizontalAxis == 0.0f) Move(Globals.Direction.North);
            }
            // Down
            else if (!isAttacking && verticalAxis < 0)
            {
                Move(Globals.Direction.South);
                if (horizontalAxis == 0.0f) Move(Globals.Direction.South);
            }

            // Left
            if (!isAttacking && horizontalAxis < 0)
            {
                Move(Globals.Direction.West);
                if (verticalAxis == 0.0f) Move(Globals.Direction.West);
            }
            // Right
            else if (!isAttacking && horizontalAxis > 0)
            {
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
			switch (direction) {
				case Globals.Direction.East:
					if (!eastCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.West:
					if (!westCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.South:
					if (!southCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				case Globals.Direction.North:
					if (!northCollider.isTriggered) {
						PlantGridObject newPlant = (PlantGridObject)Instantiate (plants[plantNumber], new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
						newPlant.Rotate(direction);
					}
					break;
				default:
					break;
			}
			Globals.inventory[plantNumber]--;
			canvas.UpdateUI();
		}
	}

    public override bool TakeDamage(int damage)
    {
        /*if (this.gameObject.tag == "Player")
        {
            hpBarText.text = "HP: " + health;
        }*/
        gameObject.GetComponent<Animation>().Play("Damaged");
        canvas.UpdateHealth(health - damage);
        return base.TakeDamage(damage);
    }

    protected virtual void LateUpdate() {
        float pixelSize = Globals.pixelSize;
        Vector3 current = this.transform.position;
        current.x = Mathf.Floor(current.x / pixelSize + 0.5f) * pixelSize;
        current.y = Mathf.Floor(current.y / pixelSize + 0.5f) * pixelSize;
        current.z = Mathf.Floor(current.z / pixelSize + 0.5f) * pixelSize;
        this.transform.position = current;
    }

}
