using UnityEngine;
using System.Collections;

public class KillableByBoomerang : MonoBehaviour {


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>() && this != null)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>())
        {
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>())
        {
            Destroy(this.gameObject);
        }
    }
}
