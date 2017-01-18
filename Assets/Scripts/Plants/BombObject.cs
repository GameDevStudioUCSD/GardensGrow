using UnityEngine;
using System.Collections;

public class BombObject : MoveableGridObject {
    public int cooldownToExplode;

    private bool fuseLit;
    private int frames;

    private BombPlantObject bombPlantObject;

	// Use this for initialization
	void Start () {
		fuseLit = true;
		frames = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (fuseLit) {
			frames++;
			if (frames >= cooldownToExplode) {
				Explode();
			}
		}
	}

	void Explode() {
		Attack();
		//bombPlantObject.RegrowBomb();
		Destroy(this.gameObject);
	}

	public void setBombPlantObject(BombPlantObject bombPlant) {
		bombPlantObject = bombPlant;
	}
}
