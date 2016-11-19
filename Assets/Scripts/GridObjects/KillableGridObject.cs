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
    private KillableGridObject toKill;

    private List<KillableGridObject> killList;
    private List<KillableGridObject> hitList = new List<KillableGridObject>();
    protected bool isAttacking = false;
    protected bool isDying = false;
    private int attackFrame = 0;
    private int dyingFrame = 0;
    //do not change these without adjusting the animation timings
    private const int numAttackFrames = 26;
    private const int numDyingFrames = 11;

	// Use this for initialization
	protected virtual void Start () {
		killList = new List<KillableGridObject>();
        base.Start();
        toKill = null;
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

        else if (isAttacking) {
            EdgeTrigger attackCollider = null;

            switch (direction) {
                case Globals.Direction.South:
                    killList = southHitCollider.getKillList();
                    attackCollider = southHitCollider;
                    break;
                case Globals.Direction.East:
                    killList = eastHitCollider.getKillList();
                    attackCollider = eastHitCollider;
                    break;
                case Globals.Direction.North:
                    killList = northHitCollider.getKillList();
                    attackCollider = northHitCollider;
                    break;
                case Globals.Direction.West:
                    killList = westHitCollider.getKillList();
                    attackCollider = westHitCollider;
                    break;
            }


            // clears references to the killed object in the PlayerEdgeTrigger
            // that collided with the killed object
            for (int i = 0; i < killList.Count; i++) {
                if (!hitList.Contains(killList[i]) && killList[i].faction != this.faction) {
                    hitList.Add(killList[i]);
                    if (killList[i].TakeDamage(damage)) {
                        if (attackCollider != null)
                            attackCollider.removeFromList(killList[i]);
                        attackCollider.isTriggered = false;
                    }
                }
            }
            attackFrame++;
            if (attackFrame >= numAttackFrames) {
                isAttacking = false;
                attackFrame = 0;
                hitList.Clear();
            }
        }
	}

	// returns true if the attack kill the object
    public virtual bool TakeDamage (int damage) {
       
        health -= damage;

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

    protected virtual void OnValidate()
    {
        TakeDamage(0);
    }

    protected virtual void Attack()
    {
        isAttacking = true;
    }

}
