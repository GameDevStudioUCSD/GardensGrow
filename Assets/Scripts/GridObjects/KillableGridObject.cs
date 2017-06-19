using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillableGridObject : RotateableGridObject {
    public LootTable lootTable;

    public int health = 20;
    public int damage = 5;
    public bool bombable = false;

    public GameObject deathPanel;
    public AttackCollider southHitCollider;
    public AttackCollider westHitCollider;
    public AttackCollider northHitCollider;
    public AttackCollider eastHitCollider;
    public Globals.Faction faction = Globals.Faction.Ally;

    public Text hpBarText;

    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hurtSound;

    // Currently isAttacking is not being used
    public bool isAttacking = false;
    public bool isDying = false;
    public bool isInvulnerable = false;

    protected List<KillableGridObject> killList;
    protected bool hitSomething;

    private int dyingFrame = 0;
    //do not change these without adjusting the animation timings
    protected const int numDyingFrames = 9;

    // Prevents "Die" function from being called more than once if something is taking continuous damage
    protected bool hasDied = false;

    protected SpriteRenderer renderer;
    private TileMap tm = null;

    // Use this for initialization
    protected override void Start() {
        base.Start();
		renderer = this.GetComponent<SpriteRenderer>();
    }

    //  is called once per frame
    protected override void Update() {
        if (isDying) {
            dyingFrame++;
            if (dyingFrame >= numDyingFrames && !this.gameObject.GetComponent<PlayerGridObject>())
            {
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
    public virtual bool TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return false;
        }

        if (!bombable)
        {
            Animation animation = gameObject.GetComponent<Animation>();

            health -= damage;

            //player damaged sound and animation handled in player
            if (audioSource != null && !this.gameObject.GetComponent<PlayerGridObject>())
            {
                audioSource.clip = hurtSound;
                audioSource.Play();

                if (animation) animation.Play("Damaged");
            }

            if (health <= 0 && hasDied == false)
            {
                if (this.gameObject.GetComponent<PlayerGridObject>())
                {
                    DieNoDrop();
                    return true;
                }
                else
                {
                    Die();
                    return true;
                }
            }
        }

        return false;
    }

    public virtual bool TakeScriptedDamage(int damage) {
        Animation animation = gameObject.GetComponent<Animation>();
        if (animation) animation.Play("Damaged");

        if (!bombable) {
            health -= damage;

            if (audioSource != null) {
                audioSource.clip = hurtSound;
                audioSource.Play();
            }

            if (health <= 0 && hasDied == false) {
                if (this.gameObject.GetComponent<PlayerGridObject>()) {
                    DieNoDrop();
                    return true;
                }
                else {
                    DieNoDrop();
                    return true;
                }
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

    public virtual bool TakeBombDamagePlayer(int damage)
    {
        if (isInvulnerable)
        {
            return false;
        }

        Animation animation = gameObject.GetComponent<Animation>();
        if (animation) animation.Play("Damaged");

        health -= damage;

        if (audioSource != null)
        {
            audioSource.clip = hurtSound;
            audioSource.Play();
        }

        if (health <= 0 && hasDied == false)
        {
            DieNoDrop();
            return true;
        }

        return false;
    }

    protected virtual void Die() {
        hasDied = true;

        if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "EnemySpawner" || this.gameObject.GetComponent<PlantGridObject>()) {
            SpawnItem();
        }

        Animator animator = this.gameObject.GetComponent<Animator>();
        /*The following line was commented out becaused it caused a bug:
         * It causes for bomb plants and bananarangs (both normal and evil) to be unplantable
         * Suggestion: maybe make sure all killable objects have a SetTrigger("Death") before uncommenting the line
         */
        //animator.SetTrigger("Death");


        isDying = true;
    }
    public IEnumerator screenBlackout(PlayerGridObject player)
    {
        //reset the player
        player.canMove = false;
        Globals.playerHealth = 12;
        player.health = Globals.playerHealth;
        player.canvas.UpdateHealth(Globals.playerHealth);

        //transitioning
        deathPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //Application.LoadLevel(Application.loadedLevel);


        player.gameObject.transform.position = Globals.spawnLocation;
        deathPanel.SetActive(false);
        player.canMove = true;

    }
    protected virtual void DieNoDrop()
    {
        if (this.gameObject.tag == "Building")
        {
            hasDied = true;
            isDying = true;
        }

        if (this.gameObject.tag == "Player")
        {
            if (!tm)
            {
                tm = FindObjectOfType<TileMap>();
            }
            StartCoroutine(screenBlackout(this.gameObject.GetComponent<PlayerGridObject>()));
        }
        //Application.LoadLevel(Application.loadedLevel);


    }
    public virtual void Attack() {
        if (!Globals.canvas.dialogue) {
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
                    }
                    hitSomething = true;
                    target.TakeDamage(damage);
                }
                if (this.GetComponent<PlayerGridObject>()) {
                    BombObject bomb = target.GetComponent<BombObject>();

                    if (bomb && !bomb.evil) {
                        bomb.Roll(direction);
                    }
                    //deplant code MOVED so deplanting is a different button
                    /*PlantGridObject plant = target.GetComponent<PlantGridObject>();
                    if (plant) {
                        plant.TakeDamage(100);
                    }*/
                }
            }
        }
    }

    public void SpawnItem() {
        if (lootTable) {
            GameObject droppedItem = lootTable.GetItem();

            // If there was a dropped item, create it
            if (droppedItem)
                Instantiate(droppedItem, transform.position, Quaternion.identity);
        }
    }

    public bool HitSomething()
    {
        return hitSomething;
    }

    protected void makeInvulnerable() {
    	isInvulnerable = true;
		renderer.color = new Color(0.5f, 0.5f, 0.5f);
    }

    protected void makeVulnerable() {
    	isInvulnerable = false;
		renderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
