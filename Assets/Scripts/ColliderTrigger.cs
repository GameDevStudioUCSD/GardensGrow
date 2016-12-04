using UnityEngine;
using System.Collections;

public class ColliderTrigger : MonoBehaviour
{
    public bool Trigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
            Trigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
            Trigger = false;
    }
}
