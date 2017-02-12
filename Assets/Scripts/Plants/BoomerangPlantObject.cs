using UnityEngine;
using System.Collections;

public class BoomerangPlantObject : PlantGridObject {

    [SerializeField]
    private BoomerangPlantProjectileObject boomerangScript;

    void Start () {
    }

    void Update () {
    }

    private void OnBecameVisible() {
        boomerangScript.AddPlant(transform.position);
    }

    private void OnBecameInvisible() {
        boomerangScript.RemovePlant(transform.position);
    }
}
