using UnityEngine;
using System.Collections;

public class RangedEnemyProjectile : MonoBehaviour {

    public float shotSpeed;
    public int Damage;

    public int shotRange;
    private int shotRangeCounter=0;
    public Globals.Direction dir;


    void Update()
    {
        if (shotRangeCounter < shotRange)
        {
            Mover(shotSpeed, dir);
            shotRangeCounter++;
        }
        else if (this.gameObject != null)
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
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerGridObject>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
        
        if (col.gameObject.GetComponent<TerrainObject>())
        {
            if (col.gameObject.GetComponent<TerrainObject>().isBarrier)
            {
                Destroy(this.gameObject);
            }
        }
        /**
        else if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemySpawner"))
        {
            Destroy(this.gameObject);
        }**/
    }
}
