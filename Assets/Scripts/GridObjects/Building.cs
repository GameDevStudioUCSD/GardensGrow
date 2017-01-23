using UnityEngine;
using System.Collections;

public class Building : KillableGridObject{

	Animator animator;
	// Use this for initialization
	protected override void Start () {
		animator = GetComponent <Animator>();
		if (animator)
			animator.SetInteger ("health", health);
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (animator)
			animator.SetInteger ("health", health);
	}
}
