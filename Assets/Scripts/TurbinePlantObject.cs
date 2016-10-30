using UnityEngine;
using System.Collections;

public class TurbinePlantObject : PlantGridObject
{

    public float speed;
    private float moveNum;
    private EnemyGridObject enemy;
    private Globals.Direction enemyDir;

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
                enemyDir = Globals.Direction.South;
            }
           else if (eastCollider.IsTouching(other))
            {
                enemyDir = Globals.Direction.East;
            }
            else if (northCollider.IsTouching(other))
            {
                enemyDir = Globals.Direction.North;
            }
            else if(westCollider.IsTouching(other))
            {
                enemyDir = Globals.Direction.West;
            }

            if(enemyDir == this.direction)
            {
                StartCoroutine(Mover(enemyDir, enemy));
            }
             
        }
    }
    IEnumerator Mover(Globals.Direction direction, EnemyGridObject enemy)
    {
       if (direction == Globals.Direction.South)
        {
            moveNum = (56.0f - (-1.0f)*enemy.GetComponent<Transform>().position.y * 12.0f) / 0.525f;
        }
        else if (direction == Globals.Direction.East)
        {
            moveNum = (56.0f - (-1.0f)*enemy.GetComponent<Transform>().position.x * 12.0f) / 0.525f;
        }
        if (direction == Globals.Direction.North)
        {
            moveNum = (56.0f - enemy.GetComponent<Transform>().position.y * 12.0f)/0.525f;
        }
        else if (direction == Globals.Direction.West)
        {
            moveNum = (56.0f - enemy.GetComponent<Transform>().position.x * 12.0f) / 0.525f;
        }

        for (int i = 0; i < moveNum; i++)
        {
            enemy.MoveEnemy(direction);

            yield return new WaitForSeconds(speed);
        }    
    }


}
