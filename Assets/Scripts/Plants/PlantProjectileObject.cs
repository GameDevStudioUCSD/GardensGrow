using UnityEngine;
using System.Collections;

public class PlantProjectileObject : MonoBehaviour {

    public float shotSpeed;
    public int shotRange;
    private int shotRangeCounter;
    public int damage;
    public Globals.Direction dir;

    EnemyGridObject enemy;

    // Use this for initialization
    void Start()
    {
        shotRangeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotRangeCounter < shotRange)
        {
            Mover(shotSpeed,dir);
            shotRangeCounter++;
        }
        else if(this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
    public void Mover(float shotSpeed, Globals.Direction dir)
    {
        if (dir == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize;
            this.transform.position = position;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            enemy = other.gameObject.GetComponent<EnemyGridObject>();
            enemy.TakeDamage(damage);
        }
        else if(other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
