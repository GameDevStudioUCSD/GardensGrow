using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantProjectileObject : MonoBehaviour {

    [SerializeField]
    private List<Vector3> visiblePlants;

    [SerializeField]
    private int nextPlant = -1;

    [SerializeField]
    private float speed = 1.0f;

    // Use this for initialization
    void Start() {
        visiblePlants = new List<Vector3>();
    }

    // Update is called once per frame
    void Update() {
        if (nextPlant != -1) {
            if (Vector2.Distance(transform.position, visiblePlants[nextPlant]) <= speed) {
                nextPlant = (nextPlant + 1) % visiblePlants.Count;
            }
            else {
                transform.position += (visiblePlants[nextPlant] - transform.position).normalized * speed;
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
}
