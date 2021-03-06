﻿using UnityEngine;
using System.Collections;

public class PlantProjectileObject : MonoBehaviour {

    //for final boss
    public bool evil = false;

    public float shotSpeed;
    public int shotRange;
    private int shotRangeCounter;
    public int damage;
    public Globals.Direction dir;

    KillableGridObject enemy;

    // Use this for initialization
    void Start()
    {
        shotRangeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.canvas.dialogue) {
            if (shotRangeCounter < shotRange) {
                Mover(shotSpeed, dir);
                shotRangeCounter++;
            }
            else if (this.gameObject != null) {
                Destroy(this.gameObject);
            }
        }
    }
    public void Mover(float shotSpeed, Globals.Direction dir)
    {
        if (dir == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize;
            this.transform.position = position;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if ( ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemySpawner") && !evil) || (evil && other.gameObject.tag == "Player"))
        {
            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if (evil)
            {
                other.GetComponent<PlayerGridObject>().TakeDamage(damage);
            }

            if (!killable.isInvulnerable && !evil)
            {
                Destroy(this.gameObject);
                enemy = other.gameObject.GetComponent<KillableGridObject>();
                enemy.TakeDamage(damage);
            }
        }
    }
}
