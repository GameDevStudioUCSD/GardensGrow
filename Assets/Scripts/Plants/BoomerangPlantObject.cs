using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantObject : PlantGridObject {

    private string roomId = null;

    void OnEnable() {
        roomId = Boomerang.RoomId(transform.position);
        if (!Boomerang.plants.ContainsKey(roomId)) {
            Boomerang.plants[roomId] = new List<Vector3>();
        }
        Boomerang.plants[roomId].Add(transform.position);
    }

    void OnDisable() {
        Boomerang.plants[roomId].Remove(transform.position);
        roomId = null;
    }
}
