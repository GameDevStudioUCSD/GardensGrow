using UnityEngine;
using System.Collections;

public class HostileTerrainObject : TerrainObject {
    private UIController canvas;
    public int damage = 12;
	private int framesPerHit = 10;
	private int currentFrame = 0;

	public bool activeCollider;
    private GameObject deathPanel;
    private PlayerGridObject player;


	// Use this for initialization
	protected override void Start () {
		base.Start();
        player = FindObjectOfType<PlayerGridObject>();
        canvas = FindObjectOfType<UIController>();
        deathPanel = canvas.deathPanelUI;

		if (!activeCollider) {
			BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
			thisCollider.enabled = false;
			Destroy(transform.GetComponent<Rigidbody>());
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			if (player.onPlatform == false) {
				player.TakeDamage(damage);
                //player.gameObject.transform.position = Globals.spawnLocation;

                player.canMove = false;
                StartCoroutine(screenBlackout());
			}
		}
        RollingBoulder boulder = other.GetComponent<RollingBoulder>();
        if (boulder) boulder.StartCrumbling();
	}
    IEnumerator screenBlackout()
    {
        //replace the following with a transparent animation later
        deathPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        player.gameObject.transform.position = Globals.spawnLocation;
        deathPanel.SetActive(false);
        player.canMove = true;

    }
}
