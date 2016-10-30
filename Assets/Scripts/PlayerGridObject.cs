﻿using UnityEngine;
using System.Collections;

public class PlayerGridObject : MoveableGridObject {

    // Use this for initialization
    protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update();
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Globals.Direction.South);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Globals.Direction.West);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(Globals.Direction.North);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Globals.Direction.East);
        }
        else if (!(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow)))
            Stop();
	}
    protected virtual void LateUpdate() {
        float pixelSize = Globals.pixelSize;
        Vector3 current = this.transform.position;
        current.x = Mathf.Floor(current.x / pixelSize + 0.5f) * pixelSize;
        current.y = Mathf.Floor(current.y / pixelSize + 0.5f) * pixelSize;
        current.z = Mathf.Floor(current.z / pixelSize + 0.5f) * pixelSize;
        this.transform.position = current;
    }
}
