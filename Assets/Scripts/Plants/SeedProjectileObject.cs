using UnityEngine;
using System.Collections;

public class SeedProjectileObject : MoveableGridObject {

    // Use this for initialization
    EnemyGridObject enemy;

	void Start () {
        base.Start();
        //animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        base.Update();
        //enemyGridObject.TakeDamage(10);
    }
    void onTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            enemy = other.GetComponent<EnemyGridObject>();
            Destroy(other.gameObject); //testing code
            //enemy.TakeDamage(10); //real code
            Destroy(this.gameObject);
        }
    }
}
