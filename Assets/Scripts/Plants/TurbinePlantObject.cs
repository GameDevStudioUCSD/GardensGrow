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

    // Use this for initialization
    protected override void Start()
    {
		// setting direction for corresponding animation
		animator = GetComponent <Animator> ();
        setDirection();
    }

    // Update is called once per frame
	protected override void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        base.Update();
	}

	public void setDirection ()
	{
		switch (this.direction) {
			case Globals.Direction.North:
				southCollider.enabled = false;
				eastCollider.enabled = false;
				northCollider.enabled = true;
				westCollider.enabled = false;
				directionalCollider = northCollider;
				break;
			case Globals.Direction.South:
				southCollider.enabled = true;
				eastCollider.enabled = false;
				northCollider.enabled = false;
				westCollider.enabled = false;
				directionalCollider = southCollider;
				break;
			case Globals.Direction.East:
				southCollider.enabled = false;
				eastCollider.enabled = true;
				northCollider.enabled = false;
				westCollider.enabled = false;
				directionalCollider = eastCollider;
				break;
			case Globals.Direction.West:
				southCollider.enabled = false;
				eastCollider.enabled = false;
				northCollider.enabled = false;
				westCollider.enabled = true;
				directionalCollider = westCollider;
				break;
		}
		animator.SetInteger ("Direction", (int)direction);	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        MoveableGridObject otherGridObject = other.GetComponent<MoveableGridObject>();
        if (otherGridObject)
        {
           	otherGridObject.Move(direction);
           	EnemyGridObject enemyGridObject = other.gameObject.GetComponent<EnemyGridObject>();
            if (enemyGridObject)
            {
            	enemyGridObject.TakeDamage(damage);
            }
        }
    }
}
