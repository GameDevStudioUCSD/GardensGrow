﻿using UnityEngine;
using System.Collections;

public class KillableGridObject : RotateableGridObject {

    public int health = 100;

	// Use this for initialization
	protected virtual void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        base.Update();
	}

    public virtual void TakeDamage (int damage) {
        health -= damage;
        if (health <= 0)
            Die();
    }

    protected virtual void Die() {
        Debug.Log("death");
        Destroy(gameObject);
    }
}
