using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LavaBossAI : KillableGridObject {
	public List<Sprite> EmergeFrames;
	public List<Sprite> IdleFrames;
	private const float ANIMATION_SPEED = 0.3f;

	public GameObject[] spawners;
	public Vector3[] boatLocations;
	public GameObject boat;
	public GameObject portal;
	private List<Fireball> fireballs = new List<Fireball>();
	public Fireball fireball;
	//public BoxCollider2D collider;
	//public SpriteRenderer sprite;

	private List<GameObject> instantiatedBoats = new List<GameObject>();

	private int currentSpawnerIndex;

	public enum BossState { Dormant, StartEmerge, Emerging, Emerged, Enraged, StartHide, Hiding };
	private BossState state;

	private const int MIN_DAMAGE = 50;
	private const int ENRAGE_HEALTH = 100;
	private const int FIREBALL_MIN_INTERVAL = 10;
	private const int FIREBALL_MAX_INTERVAL = 300;
	private const float BOSS_TIMER_DURATION = 7f;

	// Use this for initialization
	protected override void Start() {
		base.Start();
		currentSpawnerIndex = -1; // so boss randomly spawns in any corner
		state = BossState.StartEmerge;
		//state = BossState.Dormant;
		//SpawnEnemies();
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
		int numSpawns = 0;
		EnemySpawner current;
		if (state == BossState.Hiding) {
			SpawnEnemies();
			state = BossState.Dormant;
		}

		if (state == BossState.Dormant) {
			for (int i = 0; i < 4; i++) {
				current = spawners[i].GetComponent<EnemySpawner>();
				numSpawns += current.numSpawns();
            }
            Debug.Log(numSpawns);

            // Prod behavior
            if (numSpawns <= 0) {
				state = BossState.StartEmerge;
			}

		}

		// Need StartEmerge so we don't keep calling emerging while waiting for animation to finish
		if (state == BossState.StartEmerge) {
			int evenOrOdd = (int)Random.Range(0,2);
			for (int i = 0; i < 4; i++) {
				if (i % 2 == evenOrOdd) {
					instantiatedBoats.Add((GameObject)Instantiate(boat, boatLocations[i], Quaternion.identity));
				}
			}

			Emerge();
			state = BossState.Emerging;
		}
	}

	void SpawnEnemies() {
		// Spawn enemies
        for(int i=0; i<spawners.Length; i++)
        {
            spawners[i].GetComponent<EnemySpawner>().SpawnAtOnce();
        }
	}

	void Hide() {
		// Destroy boats
		int i = 0;
		while (i < instantiatedBoats.Count)
        {
        	GameObject curBoat = instantiatedBoats[i];
			instantiatedBoats.RemoveAt(i);

        	if (curBoat) {
				PlatformGridObject thisBoat = curBoat.GetComponent<PlatformGridObject>();
				thisBoat.Destructor();
        	}
        }

        // Destroy fireballs
        i = 0;
        while (i < fireballs.Count)
        {
        	Fireball fireball = fireballs[i];
			fireballs.RemoveAt(i);

        	if (fireball) {
        		Destroy(fireball.gameObject);
        	}
        }

		// Hide boss
		isInvulnerable = true;
		state = BossState.StartHide; // to stop idle animation

		StartCoroutine(HideAnimation());
	}

	IEnumerator HideAnimation() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = EmergeFrames.Count - 1; i  >= 0; i--) {
			spriteRenderer.sprite = EmergeFrames[i];

			yield return new WaitForSeconds(ANIMATION_SPEED);
		}

		FinishHide();
	}

	void FinishHide() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = false;
		state = BossState.Hiding;

		// 'Enables' the spawner that the boss was formerly at
		spawners[currentSpawnerIndex].SetActive(true);
	}


	// Makes the boss "take over" a spawner and move to the new location"
	void Emerge() {
		// Chooses a spawner index 0-3
		int newSpawnerIndex = currentSpawnerIndex;

		// Guarantees spawn in new location
		while (newSpawnerIndex == currentSpawnerIndex)
		{
			newSpawnerIndex = (int)Random.Range(0,4);
		}

		// 'Disables' the spawner so that the boss can take over it
		spawners[newSpawnerIndex].SetActive(false);

		// Moves the boss to the spawner's location
		Vector3 newPosition = spawners[newSpawnerIndex].gameObject.transform.position;
		newPosition.y++;

		currentSpawnerIndex = newSpawnerIndex;

		this.transform.position = newPosition;

		StartCoroutine(EmergeAnimation());
	}

	IEnumerator EmergeAnimation() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = true;
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = true;

		for (int i = 0; i  < EmergeFrames.Count; i++) {
			spriteRenderer.sprite = EmergeFrames[i];

			yield return new WaitForSeconds(ANIMATION_SPEED);
		}

		FinishEmerge();
	}

	void FinishEmerge() {
		isInvulnerable = false;
		state = health > ENRAGE_HEALTH ? BossState.Emerged : BossState.Enraged;

		StartCoroutine(IdleAnimation());
		StartCoroutine(BossTimer());

		StartCoroutine(SummonFireballs());
	}

	IEnumerator SummonFireballs() {
		int numToSummon = health > ENRAGE_HEALTH ? 3 : 6;
		for (int i = 0; i < numToSummon; i++) {
			yield return new WaitForSeconds(Random.Range(FIREBALL_MIN_INTERVAL, FIREBALL_MAX_INTERVAL)/1000.0f);

			// Create fireballs
			// temp fireball location

			Vector3 dropPosition = new Vector3(Random.Range(-4, 4), Random.Range(-2, 2), 0.0f);
			Fireball fireballObj = (Fireball)Instantiate(fireball, dropPosition, Quaternion.identity);
			fireballs.Add(fireballObj);
			//spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
    		//GameObject enemyObj = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
		}
	}


	IEnumerator IdleAnimation() {
		int i = 1;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		while (state == BossState.Emerged || state == BossState.Enraged) {
			spriteRenderer.sprite = IdleFrames[i % IdleFrames.Count];
			i++;

			yield return new WaitForSeconds(ANIMATION_SPEED);
    	}
	}

	public override bool TakeDamage(int dmg) {
    	if (dmg < MIN_DAMAGE) return false; // Ensures damage from boat only

    	if (state == BossState.Emerged ||
    		state == BossState.Enraged)
    	{
        	bool ret = base.TakeDamage(dmg);
			Hide();
			return ret;
        }

        return false;
    }

    protected override void Die() {
        portal.SetActive(true);
        Globals.lavaBossBeaten = true;
    	base.Die();
    }

	IEnumerator BossTimer() {
		yield return new WaitForSeconds(BOSS_TIMER_DURATION);

		if (health > 0 && (state == BossState.Emerged || state == BossState.Enraged))
        {
            Hide();
        }
	}
}
