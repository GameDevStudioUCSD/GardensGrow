using UnityEngine;
using System.Collections;

public class SeedProjectileObject : MonoBehaviour {

    public int shotSpeed;
    public int shotRange;
    private int shotRangeCounter;
    public int damage;

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
            Mover(shotSpeed);
            shotRangeCounter++;
        }
        else if(this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
    public void Mover(int shotSpeed)
    {
        Vector3 position = this.transform.position;
        position.y += Globals.pixelSize * shotSpeed;
        this.transform.position = position;
        /**if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize;
            this.transform.position = position;
        }**/
    }
    void OnTriggerEnter(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            enemy = other.gameObject.GetComponent<EnemyGridObject>();
            enemy.TakeDamage(damage);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
