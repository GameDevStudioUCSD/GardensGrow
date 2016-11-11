using UnityEngine;

using UnityEngine.UI;
using System.Collections;


public class KillableGridObject : RotateableGridObject {

    private int health = 100;
	public PlayerEdgeTrigger southHitCollider;
	public PlayerEdgeTrigger westHitCollider;
	public PlayerEdgeTrigger northHitCollider;
	public PlayerEdgeTrigger eastHitCollider;
    
    public Text hpBarPlayerText;

	// Use this for initialization
	protected virtual void Start () {
        
        base.Start();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        base.Update();
	}

    public virtual void TakeDamage (int damage) {
       
        if(health >= damage)
            health -= damage;

        if (this.gameObject.tag == "Player")
        {
            hpBarPlayerText.text = "HP: " + health;
        }
        if (health <= 0)
            Die();
    }

    protected virtual void Die() {
        Debug.Log("death");
        if(this.gameObject.tag == "Player")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    protected virtual void OnValidate()
    {
        TakeDamage(0);
    }
}
