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

        //TODO: for testing uncomment the following
        /*for(int i=0; i<Globals.unlockedSeeds.Length; i++)
        {
            Globals.unlockedSeeds[i] = true;
        }*/
    }

    void OnDisable() {
        Boomerang.boomerangs[Boomerang.RoomId(transform.position)].RemovePlant(transform.position);


    }
}
