using UnityEngine;
using System.Collections;

public class StaticGridObject : GridObject {

	public bool isBarrier;
	public BoxCollider2D barrier;

	// Use this for initialization
	void Start () {
		if (!isBarrier)
		{
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
