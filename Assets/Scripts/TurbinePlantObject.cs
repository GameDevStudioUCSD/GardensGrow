using UnityEngine;
using System.Collections;

public class TurbinePlantObject : PlantGridObject
{

    public float speed;
    private float moveNum;
    private EnemyGridObject enemy;


    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;

    // Use this for initialization
    void Start()
    {
        health = 20;
    }

    // Update is called once per frame
    public void Attack()
    {

    }
    void Update()
    {
          
        
    }
    void OnTriggerEnter2D(Collider2D other)
    { 

        if (other.GetComponent<EnemyGridObject>())
        { 
            enemy = other.GetComponent <EnemyGridObject>();

           if (southCollider.IsTouching(other))
           {
                StartCoroutine(Mover(Globals.Direction.South, enemy));
            }
           if (eastCollider.IsTouching(other))
            {
                StartCoroutine(Mover(Globals.Direction.East, enemy));
            }
            if (northCollider.IsTouching(other))
            {
                StartCoroutine(Mover(Globals.Direction.North, enemy));
            }
            if(westCollider.IsTouching(other))
            {
                StartCoroutine(Mover(Globals.Direction.West, enemy));
            }
        }
    }
    IEnumerator Mover(Globals.Direction direction, EnemyGridObject enemy)
    {
        if (direction == Globals.Direction.South)
        {
            moveNum = (48.0f - (-1.0f)*enemy.GetComponent<Transform>().position.y * 6.25f) / 6.25f;
        }
        else if (direction == Globals.Direction.East)
        {
            moveNum = (48.0f - (-1.0f)*enemy.GetComponent<Transform>().position.x * 6.25f) / 6.25f;
        }
        else if (direction == Globals.Direction.North)
        {
            moveNum = (48.0f - enemy.GetComponent<Transform>().position.y * 6.25f) / 6.25f;
        }
        else if (direction == Globals.Direction.West)
        {
            moveNum = (48.0f - enemy.GetComponent<Transform>().position.x * 6.25f) / 6.25f;
        }

       // moveNum = (48.0f - enemy.GetComponent<Transform>().position.y*6.25f)/6.25f;

        for (int i = 0; i < moveNum; i++)
        {
            enemy.MoveEnemy(direction);

            yield return new WaitForSeconds(speed);
        }    
    }


}
