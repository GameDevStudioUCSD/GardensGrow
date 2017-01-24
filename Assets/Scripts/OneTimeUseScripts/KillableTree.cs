using UnityEngine;
using System.Collections;

public class KillableTree : MonoBehaviour {

    public EnemySpawner spawner;

    /// <summary>
    /// Quick hack for the overworld, when these trees are destroyed turn on the spawner
    /// The trees were blocking the way and causing errors for the spawned monsters.
    /// TODO: we need some kind of event manager for these types of events in the game.
    /// </summary>
    void TurnOnSpawner()
    {
        if (spawner)
            spawner.BeginSpawning();
    }

	void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponentInParent<PlayerGridObject>())
        {
            if (col.gameObject.GetComponentInParent<PlayerGridObject>().isAttacking)
            {
                TurnOnSpawner();
                Destroy(this.gameObject);
            }
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<PlayerGridObject>().isAttacking)
            {
                TurnOnSpawner();
                Destroy(this.gameObject);
            }
        }
    }
}
