using UnityEngine;
using System.Collections;

public class Building : KillableGridObject{

	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
		if (animator)
			animator.SetInteger ("health", health);
	}
	
	// Update is called once per frame
	void Update () {
		if (animator)
			animator.SetInteger ("health", health);
	}
}
