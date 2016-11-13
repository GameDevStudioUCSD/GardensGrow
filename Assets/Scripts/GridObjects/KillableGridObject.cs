using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {

    public int health;
    public int damage = 10;
	public PlayerEdgeTrigger southHitCollider;
	public PlayerEdgeTrigger westHitCollider;
	public PlayerEdgeTrigger northHitCollider;
	public PlayerEdgeTrigger eastHitCollider;
    
    public Text hpBarPlayerText;
    private KillableGridObject toKill;

    private List<KillableGridObject> killList;

	// Use this for initialization
	protected virtual void Start () {
		killList = new List<KillableGridObject>();
        base.Start();
        toKill = null;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        base.Update();
	}

    public virtual void TakeDamage (int damage) {
       
        if(health >= damage)
            health -= damage;

        /*if (this.gameObject.tag == "Player")
        {
            hpBarPlayerText.text = "HP: " + health;
        }*/
        if (health <= 0)
            Die();
    }

    protected virtual void Die() {
        //Debug.Log("death");
        if(this.gameObject.tag == "Player")
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        Destroy(this.gameObject);
    }

    protected virtual void OnValidate()
    {
        TakeDamage(0);
    }

    protected virtual void Attack()
    {
    	switch (direction)
    	{
    		case Globals.Direction.South:
    			killList = southHitCollider.getKillList();
    			break;
			case Globals.Direction.East:
    			killList = eastHitCollider.getKillList();
    			break;
			case Globals.Direction.North:
    			killList = northHitCollider.getKillList();
    			break;
			case Globals.Direction.West:
    			killList = westHitCollider.getKillList();
    			break;
    	}
    	for (int i = 0; i < killList.Count; i++)
    	{
    		killList[i].TakeDamage(damage);
    		if (killList[i] == null)
    			killList.Remove(killList[i]);
    	}
    }

}
