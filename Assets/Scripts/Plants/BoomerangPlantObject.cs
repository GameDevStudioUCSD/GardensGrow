using UnityEngine;
using System.Collections;

public class BoomerangPlantObject : PlantGridObject {

    public GameObject boomerangPrefab;

    void OnEnable() {
        string id = Boomerang.RoomId(transform.position);
        if (!Boomerang.boomerangs.ContainsKey(id)) {
            GameObject boomerang = (GameObject)Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
            Boomerang.boomerangs.Add(id, boomerang.GetComponent<Boomerang>());
        }
        Boomerang.boomerangs[id].AddPlant(transform.position);
    }

    void OnDisable() {
        Boomerang.boomerangs[Boomerang.RoomId(transform.position)].RemovePlant(transform.position);
    }
}
