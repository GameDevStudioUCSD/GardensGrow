using UnityEngine;
using System.Collections;

public class Fireball : StaticGridObject
{
	public int preFallTime = 100;

	private float fallingSpeed;

	public float FALLING_SPEED_MAX = 30f;
	public float FALLING_SPEED_MIN = 10f;

	private float FALLING_SPEED_INCR = 200f;
	private float eps;

	private enum FireballState { PreFalling, Falling, Fallen }
	private FireballState state;

	public GameObject ball;

	private PlayerGridObject player;
	private int fallFrames;

	protected override void Start() {
		fallingSpeed = Random.Range(FALLING_SPEED_MIN, FALLING_SPEED_MAX) / FALLING_SPEED_INCR;
		state = FireballState.PreFalling;
		fallFrames = 0;
		eps = FALLING_SPEED_MAX / FALLING_SPEED_INCR;

		base.Start();
	}

	// Update is called once per frame
	void Update() {
		if (state == FireballState.PreFalling) {
			if (fallFrames >= preFallTime) {
				state = FireballState.Falling;
			}
			else {
				fallFrames++;
			}
		}
		if (state == FireballState.Fallen) {
			if (player)
				player.TakeDamage(5);
			Destroy(gameObject);

			return;
		}

		if (state == FireballState.Falling) {
			ball.SetActive(true);
			if (Mathf.Abs(ball.transform.position.y - transform.position.y) > eps) {
				Vector3 newPosition = ball.transform.position;
				newPosition.y -= fallingSpeed;
				ball.transform.position = newPosition;
			} else {
				ball.transform.position = transform.position;

				state = FireballState.Fallen;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			player = col.GetComponentInParent<PlayerGridObject>();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			player = null;
		}
	}
}

