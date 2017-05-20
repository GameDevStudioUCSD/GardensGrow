using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ItemDrop : StaticGridObject {

	// The ID for the drop, 8 for health pickup, 0-7 for plants
	public int itemId;
	// The sound bite for when the player picks up the item
	public AudioClip clip;
	public bool permanent;
	public int lifeSpan;

    private float x;
    private float y;
    private float z;
    
    private int life;

    //save slot stuff
    private bool pickedUp = false;

	// Use this for initialization
	protected override void Start () {
        life = 0;
    }
	
    void OnEnable()
    {
        PlayerGridObject p = FindObjectOfType<PlayerGridObject>();

        if (!p.itemsRePickUp)
        {
            x = this.gameObject.transform.position.x;
            y = this.gameObject.transform.position.y;
            z = this.gameObject.transform.position.z;

            pickedUp = PlayerPrefsX.GetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                + "pos x" + x + "pos y" + y + "pos z" + z, pickedUp);

            if (pickedUp)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    void OnDisable()
    {

        PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                  + "pos x" + x + "pos y" + y + "pos z" + z, pickedUp);

    }
	// Update is called once per frame
	void Update () {
		life++;
		if (permanent == false && life > lifeSpan) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
            pickedUp = true;
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
            if (player == null) return; // Ignore the player's other colliders (hacky)
			UIController controller = player.canvas;

            //finalbossseed is 10
            if(itemId == 10)
            {
                //not sure what's correct to write here
                Globals.inventory[8]++;
                controller.UpdateUI();
            }
            // Key is 9
			if (itemId == 9) {
				Globals.numKeys++;
				controller.UpdateUI();
			}
            // Health is 8
			else if (itemId == 8) {
				player.health++;
				if (player.health > 12)
					player.health = 12;
				controller.UpdateHealth(player.health);
			}
            // Seeds are 0 - 7
			else if (Globals.inventory[itemId] < 9) {
				Globals.inventory[itemId]++;
				controller.UpdateUI();
			}

			AudioSource.PlayClipAtPoint(clip, player.gameObject.transform.position);
			Destroy(this.gameObject);
		}
	}
}
