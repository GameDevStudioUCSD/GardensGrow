using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangPlantObject : PlantGridObject {

    private string roomId = null;

    //for final boss
    public bool evil = false;

    private Boomerang[] booms;
    private bool evilSet = false;

    protected override void Update()
    {
        base.Update();
        if (evil && !evilSet)
        {
            evilSet = true;

            booms = FindObjectsOfType<Boomerang>();

            foreach(Boomerang b in booms)
            {
                b.evil = evil;
            }

        }
    }

    void OnEnable() {
        AddSelfToRoom();
    }

    void OnDisable() {
        RemoveSelfFromRoom();
    }

    public void AddSelfToRoom() {
        roomId = Boomerang.RoomId(transform.position);
        if (!Boomerang.plants.ContainsKey(roomId)) {
            Boomerang.plants[roomId] = new List<Vector3>();
        }
        Boomerang.plants[roomId].Add(transform.position);
    }

    public void RemoveSelfFromRoom() {
        Boomerang.plants[roomId].Remove(transform.position);
        roomId = null;
    }
}
