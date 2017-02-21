using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boomerang : MonoBehaviour {

    public static Dictionary<string, Boomerang> boomerangs = new Dictionary<string, Boomerang>();

    [SerializeField]
    private List<Vector3> plants;

    [SerializeField]
    private List<GameObject> itemHeld;

    [SerializeField]
    private int nextPlant = -1;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private int damage = 1;

    public static string RoomId(Vector3 pos) {
        int x = Mathf.RoundToInt(pos.x / 14.0f) * 14;
        int y = Mathf.RoundToInt(pos.y / 10.0f) * 10;
        return string.Format("{0}{1:D5}{2:D5}", Application.loadedLevelName, x, y);
    }

    // Update is called once per frame
    void Update() {
        if (nextPlant != -1) {
            if (Vector2.Distance(transform.position, plants[nextPlant]) <= speed) {
                nextPlant = (nextPlant + 1) % plants.Count;
            }
            else {
                Vector3 dt = (plants[nextPlant] - transform.position).normalized * speed;
                dt /= Globals.pixelSize;
                dt = new Vector3(Mathf.Round(dt.x), Mathf.Round(dt.y), 0);
                dt *= Globals.pixelSize;
                transform.position += dt;
                for (int i = itemHeld.Count - 1; i >= 0; --i) {
                    // a GameObject is equal to null if it is destroyed
                    if (itemHeld[i] == null) {
                        itemHeld.RemoveAt(i);
                    }
                    else {
                        itemHeld[i].transform.position += dt;
                    }
                }
            }
        }
    }

    public void AddPlant(Vector3 plant) {
        plants.Add(plant);
        if (plants.Count == 1) {
            nextPlant = 0;
        }
    }

    public void RemovePlant(Vector3 plant) {
        plants.Remove(plant);
        if (nextPlant >= plants.Count) {
            nextPlant = 0;
        }
        else if (plants.Count <= 0) {
            Destroy(gameObject);
            boomerangs.Remove(RoomId(transform.position));
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemySpawner") {
            other.gameObject.GetComponent<KillableGridObject>().TakeDamage(damage);
        }
        if (other.gameObject.GetComponent<ItemDrop>() != null) {
            itemHeld.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<ItemDrop>() != null) {
            itemHeld.Remove(other.gameObject);
        }
    }
}
