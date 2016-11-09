using UnityEngine;
using System.Collections;

public class TurbinePlantObject : Plant 
{
    public float speed;
    //    private float moveNum;
    //    private Globals.Direction enemyDir;
    //
    //    public Collider2D southCollider;
    //    public Collider2D northCollider;
    //    public Collider2D eastCollider;
    //    public Collider2D westCollider;

    //    public ConstantSpeedModifierObject one, two, three;
    public ConstantSpeedModifierObject south0, south1, south2, south3, north0, north1, north2, north3, east0, east1, east2, east3, west0, west1, west2, west3;

    private Animator animator;

    public override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    public override void Start() { 
        base.Start();
        SetDirection(direction);
    }

//    // Update is called once per frame
//    public void Attack()
//    {
//
//    }
//	protected virtual void Update() {
//
//		updateDirection ();
//		base.Update();
//
//	}
//
	public override void SetDirection (Globals.Direction dir)
	{
        base.SetDirection(dir);

	    south1.enabled = south2.enabled = south3.enabled = north1.enabled = north2.enabled = north3.enabled = east1.enabled = east2.enabled = east3.enabled = west1.enabled = west2.enabled = west3.enabled = false;
        switch (direction)
        {
            case Globals.Direction.East:
                east0.enabled = true;
                east1.enabled = true;
                east2.enabled = true;
                east3.enabled = true;
                break;
            case Globals.Direction.West:
                west0.enabled = true;
                west1.enabled = true;
                west2.enabled = true;
                west3.enabled = true;
                break;
            case Globals.Direction.North:
                north0.enabled = true;
                north1.enabled = true;
                north2.enabled = true;
                north3.enabled = true;
                break;
            case Globals.Direction.South:
                south0.enabled = true;
                south1.enabled = true;
                south2.enabled = true;
                south3.enabled = true;
                break;
        }
        animator.SetInteger ("Direction", (int)direction);	
	}
//
//    void OnTriggerEnter2D(Collider2D other)
//    { 

//        if (other.GetComponent<MoveableGridObject>())
//        { 
//            enemy = other.GetComponent <MoveableGridObject>();
//
//            if (southCollider.IsTouching(other))
//            {
//                enemyDir = Globals.Direction.South;
//            }
//            else if (eastCollider.IsTouching(other))
//            {
//                enemyDir = Globals.Direction.East;
//            }
//            else if (northCollider.IsTouching(other))
//            {
//                enemyDir = Globals.Direction.North;
//            }
//            else if(westCollider.IsTouching(other))
//            {
//                enemyDir = Globals.Direction.West;
//            }
//
//            if(enemyDir == this.direction)
//            {
//                StartCoroutine(Mover(enemyDir, enemy));
//            }
//             
//        }
//    }
//
//    IEnumerator Mover(Globals.Direction direction, MoveableGridObject enemy)
//    {
//       if (direction == Globals.Direction.South)
//        {
//            moveNum = (56.0f - (-1.0f)*enemy.GetComponent<Transform>().position.y * 12.0f) / 0.525f;
//        }
//        else if (direction == Globals.Direction.East)
//        {
//            moveNum = (56.0f - (-1.0f)*enemy.GetComponent<Transform>().position.x * 12.0f) / 0.525f;
//        }
//        if (direction == Globals.Direction.North)
//        {
//            moveNum = (56.0f - enemy.GetComponent<Transform>().position.y * 12.0f)/0.525f;
//        }
//        else if (direction == Globals.Direction.West)
//        {
//            moveNum = (56.0f - enemy.GetComponent<Transform>().position.x * 12.0f) / 0.525f;
//        }
//
//        for (int i = 0; i < moveNum; i++)
//        {
//            enemy.MoveEnemy(direction);
//
//            yield return new WaitForSeconds(speed);
//        }    
//    }
}