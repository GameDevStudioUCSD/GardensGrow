using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantProjectileObject : MonoBehaviour {

    [SerializeField]
    private List<Vector3> visiblePlants;

    [SerializeField]
    private List<GameObject> itemHeld;

    [SerializeField]
    private int nextPlant = -1;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private int damage = 1;

    // Use this for initialization
    void Start() {
        visiblePlants = new List<Vector3>();
        itemHeld = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        if (nextPlant != -1) {
            if (Vector2.Distance(transform.position, visiblePlants[nextPlant]) <= speed) {
                nextPlant = (nextPlant + 1) % visiblePlants.Count;
            }
            else {
                Vector3 dt = (visiblePlants[nextPlant] - transform.position).normalized * speed;
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
        visiblePlants.Add(plant);
        if (visiblePlants.Count == 1) {
            nextPlant = 0;
            transform.position = plant;
        }
    }

    public void RemovePlant(Vector3 plant) {
        visiblePlants.Remove(plant);
        if (visiblePlants.Count <= 0) {
            nextPlant = -1;
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
