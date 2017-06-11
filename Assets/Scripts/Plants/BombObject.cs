using UnityEngine;
using System.Collections;

public class BombObject : MoveableGridObject {

    //for final boss
    public bool evil = false;

    public int fuseFrames;

    private bool fuseLit = false;
    private int frames;
    private bool isRolling;
    private Globals.Direction rollDirection = Globals.Direction.North;

    private BombPlantObject bombPlantObject;
    private Animator animator;

    // Use this for initialization
    protected override void Start() {
        fuseLit = false;
        frames = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update() {
        if (!Globals.canvas.dialogue) {
            if (fuseLit) {
                frames++;
                if (frames >= fuseFrames) {
                    Explode();
                }
            }
            if (isRolling) {
                Move(rollDirection);
            }
        }
    }

    //call to start bomb rolling
    public void Roll(Globals.Direction direction) {
        LightFuse();
        if (!isRolling) { //remove this if statement to allow bomb to change direction mid-roll
            animator.SetInteger("Direction", (int)direction);
            animator.SetTrigger("Roll");
            isRolling = true;
            rollDirection = direction;
        }
    }

    //stop rolling
    public void Stop() {
        isRolling = false;
    }

    //call to start bomb timer
    public void LightFuse() {
        fuseLit = true;
    }

    //call to have bomb attack everything around it
    public void Explode() {
        animator.SetTrigger("Explode");
        isRolling = false;
        Attack();
    }

    //call to destroy bomb and trigger regrowth; called by animation
    public void FinishExploding() {
        bombPlantObject.RegrowBomb();
        Destroy(this.gameObject);
    }

    //hiding KillableGridObject's method; bombs don't die
    public override bool TakeDamage(int damage) {
        if (isRolling) Explode();
        return false;
    }

    //KillableGridObject's Attack(), but calls TakeBombDamage() instead of TakeDamage()
    public override void Attack() {
        // Don't attack if we are currently attacking
        if (isAttacking)
            return;

        isAttacking = true;

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
            if (target.GetComponent<PlayerGridObject>())
            {
                target.TakeBombDamagePlayer(damage);
            }
            else
            {
                target.TakeBombDamage(damage);
            }
        }

    }

    public void setBombPlantObject(BombPlantObject bombPlant) {
        bombPlantObject = bombPlant;
    }

    //MoveableGridObject's Move(), plus stops rolling on obstacle collision and explodes on enemy collision
    public override void Move(Globals.Direction direction) {
        if (!(Globals.player.canvas.dialogue)) {
            base.Move(direction);
            if (direction == Globals.Direction.North && northCollider.isTriggered && northCollider.other.GetComponent<WindBossAI>())
                Explode();
            else if (direction == Globals.Direction.South && southCollider.isTriggered && southCollider.other.GetComponent<WindBossAI>())
                Explode();
            else if (direction == Globals.Direction.West && westCollider.isTriggered && westCollider.other.GetComponent<WindBossAI>())
                Explode();
            else if (direction == Globals.Direction.South && eastCollider.isTriggered && eastCollider.other.GetComponent<WindBossAI>())
                Explode();
            //this code makes the bomb explode automatically when an enemy is range
            /*if (direction == Globals.Direction.South) {
                killList = southHitCollider.GetKillList();
                foreach (KillableGridObject other in killList) {
                    if (other.gameObject.GetComponent<KillableGridObject>() != null) {
                        if (other.gameObject.GetComponent<KillableGridObject>().faction == Globals.Faction.Enemy) {
                            Explode();
                            break;
                        }
                    }
                }
                if (southCollider.isTriggered)
                    isRolling = false;
            }
            else if (direction == Globals.Direction.West) {
                killList = westHitCollider.GetKillList();
                foreach (KillableGridObject other in killList) {
                    if (other.gameObject.GetComponent<KillableGridObject>() != null) {
                        if (other.gameObject.GetComponent<KillableGridObject>().faction == Globals.Faction.Enemy) {
                            Explode();
                            break;
                        }
                    }
                }
                if (westCollider.isTriggered)
                    isRolling = false;
            }
            else if (direction == Globals.Direction.North) {
                killList = northHitCollider.GetKillList();
                foreach (KillableGridObject other in killList) {
                    if (other.gameObject.GetComponent<KillableGridObject>() != null) {
                        if (other.gameObject.GetComponent<KillableGridObject>().faction == Globals.Faction.Enemy) {
                            Explode();
                            break;
                        }
                    }
                }
                if (northCollider.isTriggered)
                    isRolling = false;
            }
            else if (direction == Globals.Direction.East) {
                killList = eastHitCollider.GetKillList();
                foreach (KillableGridObject other in killList) {
                    if (other.gameObject.GetComponent<KillableGridObject>() != null) {
                        if (other.gameObject.GetComponent<KillableGridObject>().faction == Globals.Faction.Enemy) {
                            Explode();
                            break;
                        }
                    }
                }
                if (eastCollider.isTriggered)
                    isRolling = false;
            } */
        }
    }

    public int CheckFrames() {
        return frames;
    }
}
