using UnityEngine;
using System.Collections;

public class BombPlantObject : PlantGridObject {

	public GameObject bombObject;
	public int cooldownToRegrowPlant;

	private bool noBomb;
	private BombObject bomb;
	private int frames;

	// Use this for initialization
	void Start () {
		frames = 0;
		noBomb = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (noBomb) {
			frames++;
			if (frames >= cooldownToRegrowPlant) {
				noBomb = false;
				frames = 0;
				bomb = (BombObject)Instantiate(bombObject, this.gameObject.transform.position, Quaternion.identity);
				Debug.Log(frames);
				bomb.setBombPlantObject(this);
			}
		}
	}

	public void RegrowBomb() {
		noBomb = true;
	}
}
