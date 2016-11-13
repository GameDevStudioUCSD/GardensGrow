using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {

    public int health;
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
        if(this.gameObject.tag == "Player")
        {
            health = 100;
        }
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
		PlayerEdgeTrigger attackCollider = null;
		
    	switch (direction)
    	{
			case Globals.Direction.South:
				killList = southHitCollider.getList ();
				attackCollider = southHitCollider;
	    		break;
			case Globals.Direction.East:
				killList = eastHitCollider.getList ();
				attackCollider = eastHitCollider;
	    		break;
			case Globals.Direction.North:
				killList = northHitCollider.getList ();
				attackCollider = northHitCollider;
	    		break;
			case Globals.Direction.West:
				killList = westHitCollider.getList ();
				attackCollider = westHitCollider;
	    		break;
    	}


		// clears references to the killed object in the PlayerEdgeTrigger
		// that collided with the killed object
    	for (int i = 0; i < killList.Count; i++)
    	{
			if (killList [i].TakeDamage (5)) {
				if (attackCollider != null)
					attackCollider.removeFromList (killList[i]);
			}
    	}
    }

}
