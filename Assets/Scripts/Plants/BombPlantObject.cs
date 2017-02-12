using UnityEngine;
using System.Collections;

public class BombPlantObject : PlantGridObject {

	public GameObject bombObject;
	public int regrowFrames;

	private bool noBomb;
	private BombObject bomb;
	private int frames;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        noBomb = false;
        frames = 0;
        GameObject newBomb = (GameObject)Instantiate(bombObject, this.gameObject.transform.position, Quaternion.identity);
        bomb = newBomb.GetComponent<BombObject>();
        bomb.setBombPlantObject(this);
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
		if (noBomb) {
			frames++;
			if (frames >= regrowFrames) {
				noBomb = false;
				frames = 0;
                GameObject newBomb = (GameObject)Instantiate(bombObject, this.gameObject.transform.position, Quaternion.identity);
                bomb = newBomb.GetComponent<BombObject>();
				bomb.setBombPlantObject(this);
			}
		}
	}

    public override bool TakeDamage(int damage) {
        if (noBomb || bomb.CheckFrames() > 26)
            return base.TakeDamage(damage);
        else return false;
    }

    public override bool TakeBombDamage(int damage) {
        return false;
    }

    public void RegrowBomb() {
		noBomb = true;
	}
}
