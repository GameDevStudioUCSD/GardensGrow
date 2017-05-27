using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinModule : MonoBehaviour {

    private List<GameObject> spinning = new List<GameObject>();
    private float rotationSpeed;
    private int framesBetweenDamage;
    private int frame;
    private int damage;

    protected virtual void Start() {
        rotationSpeed = GetComponentInParent<SpinningPlant>().rotationSpeed;
        framesBetweenDamage = GetComponentInParent<SpinningPlant>().framesBetweenDamage;
        damage = GetComponentInParent<SpinningPlant>().damage;
    }

    protected virtual void Update() {
        if (!Globals.canvas.dialogue) {
            frame++;
            spinning.RemoveAll((GameObject target) => target == null);
            foreach (GameObject spun in spinning) {
                Quaternion rotation = spun.transform.rotation;
                spun.transform.RotateAround(this.transform.position, Vector3.forward, rotationSpeed);
                spun.transform.rotation = rotation;
                if (frame >= framesBetweenDamage) {
                    KillableGridObject killable = spun.GetComponent<KillableGridObject>();
                    if (killable) killable.TakeDamage(damage);
                }
            }
            if (frame >= framesBetweenDamage) frame = 0;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        EnemyGridObject enemy = other.gameObject.GetComponent<EnemyGridObject>();
        if (enemy) {
            spinning.Add(other.gameObject);
        }
        BombObject bomb = other.gameObject.GetComponent<BombObject>();
        if (bomb) {
            bomb.LightFuse();
            bomb.GetComponent<Animator>().SetTrigger("Roll");
            spinning.Add(other.gameObject);
        }
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if (spinning.Contains(other.gameObject)) spinning.Remove(other.gameObject);
    }
}
