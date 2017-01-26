using UnityEngine;
using System.Collections;
using System;

public class Location : IComparable<Location> {

	public Vector3 coordinate;
	public string scene;

	// Use this for initialization
	void Start () {
	
	}

	public int CompareTo(Location other) {
		return 0;
	}
}
