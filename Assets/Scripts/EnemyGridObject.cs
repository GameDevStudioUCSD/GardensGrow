using UnityEngine;
using System.Collections;

public class EnemyGridObject : MoveableGridObject {
    

	// Use this for initialization
	protected virtual void Start () {

        Vector3 hpSpawn = new Vector3(GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y+2, 0.0f);
        Quaternion rt = Quaternion.identity;
        Instantiate(hpBar,hpSpawn,rt);
        

        Debug.Log("hi");
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}
}
