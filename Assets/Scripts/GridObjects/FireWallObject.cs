using UnityEngine;
using System.Collections;

public class FireWallObject : StaticGridObject {

	public virtual void Toggle() {
        if (gameObject.activeSelf) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
    /*
	void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGridObject>();
        animator = GetComponent<Animator>();
        if (!spawnsOnce) // Continuous spawn
        {
            StartCoroutine(spawnRandomDir());
        }
    }*/


}
