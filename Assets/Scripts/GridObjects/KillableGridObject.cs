using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {

    public int health = 20;
    public int damage = 5;
	public EdgeTrigger southHitCollider;
	public EdgeTrigger westHitCollider;
	public EdgeTrigger northHitCollider;
	public EdgeTrigger eastHitCollider;
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
    private const int numDyingFrames = 11;

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

		if (health <= 0) {
			Die ();
			return true;
		}

		return false;
    }

    protected virtual void Die() {
        //Debug.Log("death");
		if(this.gameObject.tag == "Player" || this.gameObject.tag == "Building") {
            Debug.Log("Player has died");
            Application.LoadLevel(Application.loadedLevel);
        }
        isDying = true;
    }

    protected virtual void Attack()
    {
        // Don't attack if we are currently attacking
        if (isAttacking)
            return;

        isAttacking = true;

		if (audio != null)
		{
			audio.clip = attackSound;
			audio.Play();
		}

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

        /*
         * Clear all dead targets in killList.
         * This uses lambda syntax: if a KillableGridObject in
         * the list is null, then remove it.
         */
        killList.RemoveAll((KillableGridObject target) => target == null);

        // Deal damage to all targets of the enemy faction
        foreach(KillableGridObject target in killList)
        {
            if(target.faction != this.faction)
            {
                target.TakeDamage(damage);
            }
        }


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

}
