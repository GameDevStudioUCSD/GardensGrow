using UnityEngine;
using System.Collections;

public class KillableTree : MonoBehaviour {

	void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponentInParent<PlayerGridObject>())
        {
            if (col.gameObject.GetComponentInParent<PlayerGridObject>().isAttacking)
            {
                Destroy(this.gameObject);
            }
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<PlayerGridObject>().isAttacking)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
