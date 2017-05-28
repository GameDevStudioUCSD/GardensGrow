using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantObject : PlantGridObject {

    private string roomId = null;

    //for final boss
    public bool evil = false;

    private Boomerang[] booms;
    protected override void Update()
    {
        base.Update();
        if (evil)
        {
            booms = FindObjectsOfType<Boomerang>();

            foreach(Boomerang b in booms)
            {
                b.evil = evil;
            }

        }
    }
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
