using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {

    public int health = 20;
    public int damage = 5;
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

	// returns true if the attack kill the object
    public virtual bool TakeDamage (int damage) {
       
        if(health >= damage)
            health -= damage;

        /*if (this.gameObject.tag == "Player")
        {
            hpBarPlayerText.text = "HP: " + health;
        }*/
		if (health <= 0) {
			Die ();
			return true;
		}

		return false;
    }

    protected virtual void Die() {
        //Debug.Log("death");
		if(this.gameObject.tag == "Player" || this.gameObject.tag == "Building")
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
		PlayerEdgeTrigger attackCollider = null;
		
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


		// clears references to the killed object in the PlayerEdgeTrigger
		// that collided with the killed object
    	for (int i = 0; i < killList.Count; i++)
    	{
			if (killList [i].TakeDamage (damage)) {
				if (attackCollider != null)
					attackCollider.removeFromList (killList[i]);
				attackCollider.isTriggered = false;
			}
    	}
    }

}
