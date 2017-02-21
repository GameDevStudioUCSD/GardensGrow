using UnityEngine;
using System.Collections;

public class CactusPlantObject : PlantGridObject {

    //public stuff
    public float punchAnimationWait;
    public float punchCoolDown;
    public int knockBackPower;

    //colliders
    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;


    //private stuff
    private bool isPunching = false;
    private Animator anim;

    protected override void Start() {
        anim = this.gameObject.GetComponent<Animator>();

        base.Start();
    }

    protected override void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        // base.Update();   //idk maybe usable
    }
    void Punch(Collider2D other, Globals.Direction dir)
    {
        if (other.gameObject.GetComponent<Switch>())
        {
            other.gameObject.GetComponent<Switch>().TakeDamage(0);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(punchAnimationWaiting(other, dir)); //changeable
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemySpawner") || other.gameObject.GetComponent<Switch>())
        {
            if (isAttacking == false)
            {
                if (other.IsTouching(southCollider.gameObject.GetComponent<BoxCollider2D>()))
                {
                    direction = Globals.Direction.South;
                    isAttacking = true;
                    Punch(other, Globals.Direction.South);
                    anim.SetInteger("Direction", 2);
                }
                else if (other.IsTouching(northCollider.gameObject.GetComponent<BoxCollider2D>()))
                {
                    direction = Globals.Direction.North;
                    isAttacking = true;
                    Punch(other, Globals.Direction.North);
                    anim.SetInteger("Direction", 0);
                }
                else if (other.IsTouching(eastCollider.gameObject.GetComponent<BoxCollider2D>()))
                {
                    direction = Globals.Direction.East;
                    isAttacking = true;
                    Punch(other, Globals.Direction.East);
                    anim.SetInteger("Direction", 1);
                }
                else if (other.IsTouching(westCollider.gameObject.GetComponent<BoxCollider2D>()))
                {
                    direction = Globals.Direction.West;
                    isAttacking = true;
                    Punch(other, Globals.Direction.West);
                    anim.SetInteger("Direction", 3);
                }
            }
        }
    }

    IEnumerator punchAnimationWaiting(Collider2D other, Globals.Direction dir)
    {
        yield return new WaitForSeconds(punchAnimationWait);
        other.gameObject.GetComponent<EnemyGridObject>().TakeDamage(damage);
        for (int i = 0; i < knockBackPower; i++)
        {
            other.gameObject.GetComponent<MoveableGridObject>().Move(dir);
        }
        StartCoroutine(punchCD());  //changeable
    }

    IEnumerator punchCD()
    {
        anim.SetInteger("Direction", 5);
        yield return new WaitForSeconds(punchCoolDown);
        isAttacking = false;
    }
}
