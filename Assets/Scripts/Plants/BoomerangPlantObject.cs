using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantObject : PlantGridObject {

    private string roomId = null;

    void OnEnable() {
<<<<<<< HEAD
        string id = Boomerang.RoomId(transform.position);
        if (!Boomerang.boomerangs.ContainsKey(id)) {
            GameObject boomerang = (GameObject)Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
            Boomerang.boomerangs.Add(id, boomerang.GetComponent<Boomerang>());
        }
        Boomerang.boomerangs[id].AddPlant(transform.position);

        //TODO: for testing uncomment the following
       /* for(int i=0; i<Globals.unlockedSeeds.Length; i++)
        {
            Globals.unlockedSeeds[i] = true;
        }*/
    }

    void OnDisable() {
        Boomerang.boomerangs[Boomerang.RoomId(transform.position)].RemovePlant(transform.position);
=======
        roomId = Boomerang.RoomId(transform.position);
        if (!Boomerang.plants.ContainsKey(roomId)) {
            Boomerang.plants[roomId] = new List<Vector3>();
        }
        Boomerang.plants[roomId].Add(transform.position);
    }

    void OnDisable() {
        Boomerang.plants[roomId].Remove(transform.position);
        roomId = null;
>>>>>>> 8fb1c2cc351c4201be12af9ed013a7425ff8937a
    }
}
