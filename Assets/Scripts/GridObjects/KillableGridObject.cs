using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {
	public ItemDrop[] itemDrops;

    public int health = 20;
    public int damage = 5;
	public AttackCollider southHitCollider;
	public AttackCollider westHitCollider;
	public AttackCollider northHitCollider;
	public AttackCollider eastHitCollider;
    public Globals.Faction faction = Globals.Faction.Ally;
    
    public Text hpBarText;

    public AudioSource audio;
    public AudioClip attackSound;
    public AudioClip hurtSound;

    protected bool isAttacking = false;
    protected bool isDying = false;

    private List<KillableGridObject> killList;

    private int attackFrame = 0;
    private int dyingFrame = 0;
    //do not change these without adjusting the animation timings
    private const int numAttackFrames = 26;
    private const int numDyingFrames = 20;//11;

    // Prevents "Die" function from being called more than once if something is taking continuous damage
    private bool hasDied = false;

	// Use this for initialization
	protected virtual void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        base.Update();
        if (isDying)
        {
            dyingFrame++;
            if (dyingFrame >= numDyingFrames)
            {
                Destroy(this.gameObject);
            }
        }

        attackFrame++;
        if (attackFrame >= numAttackFrames)
        {
            isAttacking = false;
            attackFrame = 0;
        }
	}

    /// <summary>
    /// The object has finished attacking.
    /// This should be called as an animation event.
    /// Look in the attack animations.
    /// </summary>
    public void FinishedAttack()
    {
        isAttacking = false;
    }

	// returns true if the attack kill the object
    public virtual bool TakeDamage (int damage) {

        health -= damage;

        if (audio != null)
        {
        	audio.clip = hurtSound;
        	audio.Play();
        }

		if (health <= 0 && hasDied == false) {
			Die ();
			return true;
		}

		return false;
    }

    protected virtual void Die() {
        hasDied = true;
		if(this.gameObject.tag == "Player" || this.gameObject.tag == "Building") {
            Debug.Log("Player has died");
            Application.LoadLevel(Application.loadedLevel);
        }

        if (this.gameObject.tag == "Enemy") {
        	spawnItem();
        }
        isDying = true;
    }

    protected virtual bool Attack()
    {
        // Don't attack if we are currently attacking
        if (isAttacking)
            return false;

        isAttacking = true;

		if (audio != null)
		{
			audio.clip = attackSound;
			audio.Play();
		}

        switch (direction)
        {
            case Globals.Direction.South:
                killList = southHitCollider.GetKillList();
                break;
            case Globals.Direction.East:
                killList = eastHitCollider.GetKillList();
                break;
            case Globals.Direction.North:
                killList = northHitCollider.GetKillList();
                break;
            case Globals.Direction.West:
                killList = westHitCollider.GetKillList();
                break;
        }

        /*
         * Clear all dead targets in killList.
         * This uses lambda syntax: if a KillableGridObject in
         * the list is null, then remove it.
         */
        killList.RemoveAll((KillableGridObject target) => target == null);

        bool returnVal = false;
        // Deal damage to all targets of the enemy faction
        foreach(KillableGridObject target in killList)
        {
            if(target.faction != this.faction)
            {
                if (target.TakeDamage(damage)) {
                	returnVal = true;
                }
            }
        }

        return returnVal;

        // clears references to the killed object in the PlayerEdgeTrigger
        // that collided with the killed object
        /* TODO: this is no longer used but keep it for now so we can roll back if needed
        for (int i = 0; i < killList.Count; i++)
        {
            if (!hitList.Contains(killList[i]) && killList[i].faction != this.faction)
            {
                hitList.Add(killList[i]);
                if (killList[i].TakeDamage(damage))
                {
                    if (attackCollider != null)
                        attackCollider.removeFromList(killList[i]);
                    attackCollider.isTriggered = false;
                }
            }
        }
        */

        
    }

    void spawnItem() {
    	int willSpawn = (int)Random.Range(0,Globals.chanceOfDrop);

    	if (willSpawn > 0) {
    		int numAvailableSeeds = 0;
    		int seedToSpawn = -1;
    		for (int i = 0; i < 8; i++) {
    			if (Globals.unlockedSeeds[i] == true) {
					numAvailableSeeds++;
					int probability = (int)Random.Range(0, numAvailableSeeds);
    				if (probability == 0) {
    					seedToSpawn = i;
    				}
				}
    		}
    		if (seedToSpawn != -1) {
    			Instantiate(itemDrops[seedToSpawn], this.gameObject.transform.position, Quaternion.identity);
    		}
    	}
    }
}
