using UnityEngine;
using System.Collections;

public class WeedBoss : MonoBehaviour {

    private FinalDungeonBoss f;
    private Animation anim;

    private bool canHit = true;

    // Use this for initialization
    void Start () {
        f = FindObjectOfType<FinalDungeonBoss>();
        f.spawnedMe = true;
        anim = this.gameObject.GetComponent<Animation>();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerGridObject>())
        {
            if (other.gameObject.GetComponent<PlayerGridObject>().isAttacking)
            {
                if (canHit)
                {
                    StartCoroutine(hit());
                    canHit = false;
                }
            }
        }
    }

    IEnumerator hit()
    {

        f.hp--;
        f.spawnedMe = false;
        anim.Play("Damaged");

        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
