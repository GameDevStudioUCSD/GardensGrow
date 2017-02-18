using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {
    public LootTable lootTable;

    public int health = 20;
    public int damage = 5;
    public bool bombable = false;

    public AttackCollider southHitCollider;
    public AttackCollider westHitCollider;
    public AttackCollider northHitCollider;
    public AttackCollider eastHitCollider;
    public Globals.Faction faction = Globals.Faction.Ally;

    public Text hpBarText;

    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hurtSound;

    public bool isAttacking = false;
    public bool isDying = false;
    public bool isInvulnerable = false;

    protected List<KillableGridObject> killList;
    protected bool hitSomething;

    private int dyingFrame = 0;
    //do not change these without adjusting the animation timings
    protected const int numDyingFrames = 11;

    // Prevents "Die" function from being called more than once if something is taking continuous damage
    protected bool hasDied = false;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        //base.Update();
        if (isDying) {
            dyingFrame++;
            if (dyingFrame >= numDyingFrames) {
                Destroy(this.gameObject);
            }
        }

    }

    /// <summary>
    /// The object has finished attacking.
    /// This should be called as an animation event.
    /// Look in the attack animations.
    /// </summary>
    public void FinishedAttack() {
        isAttacking = false;
    }

    // returns true if the attack kills the object
    public virtual bool TakeDamage(int damage) {
        if (isInvulnerable) {
            return false;
        }

        Animation animation = gameObject.GetComponent<Animation>();
        if (animation) animation.Play("Damaged");

        if (!bombable) {
            health -= damage;

            if (audioSource != null) {
                audioSource.clip = hurtSound;
                audioSource.Play();
            }

            if (health <= 0 && hasDied == false) {
                Die();
                return true;
            }
        }

        return false;
    }

    public virtual bool TakeBombDamage(int damage) {
        if (isInvulnerable) {
            return false;
        }

        Animation animation = gameObject.GetComponent<Animation>();
        if (animation) animation.Play("Damaged");

        health -= damage;

        if (audioSource != null) {
            audioSource.clip = hurtSound;
            audioSource.Play();
        }

        if (health <= 0 && hasDied == false) {
            Die();
            return true;
        }

        return false;
    }

    protected virtual void Die() {
        hasDied = true;
        if (this.gameObject.tag == "Player" || this.gameObject.tag == "Building") {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "EnemySpawner" || this.gameObject.GetComponent<PlantGridObject>()) {
            SpawnItem();
        }
        isDying = true;
    }

    public virtual void Attack() {
        // Don't attack if we are currently attacking
        if (isAttacking)
            return;

        isAttacking = true;
        hitSomething = false;

        if (audioSource != null) {
            audioSource.clip = attackSound;
            audioSource.Play();
        }
        switch (direction) {
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

        // Deal damage to all targets of the enemy faction
        foreach (KillableGridObject target in killList) {
            if (target.faction != this.faction) {
                if (this.gameObject.CompareTag("Enemy") && target.gameObject.GetComponent<WatermelonPlantObject>()) {
                    //note enemy kill plant doesn't work
                    Debug.Log("SMACKING THE WATERMELOON");
                }
                hitSomething = true;
                target.TakeDamage(damage);
            }
            if (this.GetComponent<PlayerGridObject>()) {
                BombObject bomb = target.GetComponent<BombObject>();
                if (bomb) {
                    bomb.Roll(direction);
                }
                PlantGridObject plant = target.GetComponent<PlantGridObject>();
                if (plant) {
                    plant.TakeDamage(100);
                }
            }
        }

    }

    public void SpawnItem() {
        /*
    	if (guaranteeDrop) {
    		Instantiate(drop, this.gameObject.transform.position, Quaternion.identity);
    	}
    	else {
	    	int willSpawn = (int)Random.Range(0,chanceOfDrop);

	    	if (willSpawn > 0) {
	    		int numAvailableSeeds = 0;
	    		int seedToSpawn = -1;
	    		for (int i = 0; i < 9; i++) {
	    			if (Globals.unlockedSeeds[i] == true && i < itemDropPercentages.Length) {
						//numAvailableSeeds++;
						numAvailableSeeds += itemDropPercentages[i];
						int probability = (int)Random.Range(0, numAvailableSeeds);
	    				//if (probability == 0) {
	    				if (probability < itemDropPercentages[i]) {
	    					seedToSpawn = i;
	    				}
					}
	    		}
	    		if (seedToSpawn != -1) {
	    			Instantiate(itemDrops[seedToSpawn], this.gameObject.transform.position, Quaternion.identity);
	    		}
	    	}
    	}
        */
        if (lootTable) {
            GameObject droppedItem = lootTable.GetItem();

            // If there was a dropped item, create it
            if (droppedItem)
                Instantiate(droppedItem, transform.position, Quaternion.identity);
        }
    }
}
