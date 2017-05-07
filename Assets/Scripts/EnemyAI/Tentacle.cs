using UnityEngine;
using System.Collections;

public class Tentacle : MonoBehaviour {

    public float speed = 1.0f;
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerGridObject>())
        {
            other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(1);
        }
    }
    public void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize*speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize*speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize * speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize * speed;
            this.transform.position = position;
        }
    }
}
