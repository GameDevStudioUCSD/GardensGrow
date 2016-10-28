using UnityEngine;
using System.Collections;

public class StaticGridObject : GridObject {

	public bool isBarrier;
	public BoxCollider2D barrier;

	// Use this for initialization
	protected virtual void Start () {
		if (!isBarrier)
		{
		}
	}
}
