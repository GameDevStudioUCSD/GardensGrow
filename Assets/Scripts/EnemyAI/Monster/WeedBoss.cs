using UnityEngine;
using System.Collections;

public class WeedBoss : MonoBehaviour {

    private FinalDungeonBoss f;

	// Use this for initialization
	void Start () {
        f = FindObjectOfType<FinalDungeonBoss>();
        f.spawnedMe = true;
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerGridObject>())
        {
            if (other.gameObject.GetComponent<PlayerGridObject>().isAttacking)
            {
                f.hp--;
                f.spawnedMe = false;
                Destroy(this.gameObject);
            }
        }
    }
}
