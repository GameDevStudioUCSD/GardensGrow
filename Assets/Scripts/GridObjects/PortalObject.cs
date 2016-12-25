using UnityEngine;
using System.Collections;

public class PortalObject : StaticGridObject {
	// 0 = Main Menu
	// 1 = Overworld
	// 2 = Lava Dungeon
	// 3 = Lava Dungeon Boss

	public int levelToLoad;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player"))
        {
        	Application.LoadLevel(levelToLoad);
        }
	}
}
