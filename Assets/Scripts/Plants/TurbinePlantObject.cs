using UnityEngine;
using System.Collections;

public class TurbinePlantObject : PlantGridObject
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

    // Use this for initialization
    void Start()
    {
		// setting direction for corresponding animation
		animator = GetComponent <Animator> ();
        setDirection();
    }

    // Update is called once per frame
    public void Attack(EnemyGridObject enemy)
    {
    	enemy.TakeDamage(10);
    }
	protected virtual void Update() {

		base.Update();
	}

	void setDirection ()
	{
		switch (this.direction) {
			case Globals.Direction.North:
				southCollider.enabled = false;
				eastCollider.enabled = false;
				westCollider.enabled = false;
				directionalCollider = northCollider;
				directionInt = 0;
				break;
			case Globals.Direction.South:
				eastCollider.enabled = false;
				northCollider.enabled = false;
				westCollider.enabled = false;
				directionalCollider = southCollider;
				directionInt = 1;
				break;
			case Globals.Direction.East:
				southCollider.enabled = false;
				northCollider.enabled = false;
				westCollider.enabled = false;
				directionalCollider = eastCollider;
				directionInt = 2;
				break;
			case Globals.Direction.West:
				southCollider.enabled = false;
				eastCollider.enabled = false;
				northCollider.enabled = false;
				directionalCollider = westCollider;
				directionInt = 3;
				break;
		}
		animator.SetInteger ("Direction", directionInt);	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        MoveableGridObject otherGridObject = other.GetComponent<MoveableGridObject>();
        if (otherGridObject)
        {
           	otherGridObject.Move(direction);
        }

        // TODO: the Attack function and take damage functions shouldn't be only in EnemyGridObject
        EnemyGridObject enemyGridObject = other.GetComponent<EnemyGridObject>();
        if(enemyGridObject)
        {
            Attack(enemyGridObject);
        }
    }
}
