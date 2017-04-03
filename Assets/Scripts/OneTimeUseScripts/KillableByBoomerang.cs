using UnityEngine;
using System.Collections;

public class KillableByBoomerang : MonoBehaviour {


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>() && this != null)
        {
            Debug.Log("have the boomerang 3");
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>())
        {
            Debug.Log("have the boomerang 1");
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Boomerang>())
        {
            Debug.Log("have the boomerang 2");
            Destroy(this.gameObject);
        }
    }
}
