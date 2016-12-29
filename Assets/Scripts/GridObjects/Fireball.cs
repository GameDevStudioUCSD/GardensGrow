using UnityEngine;
using System.Collections;

public class Fireball : StaticGridObject
{
	private float fallingSpeed;
	private const int FALLING_SPEED_MAX = 30;
	private const int FALLING_SPEED_MIN = 10;
	private const float FALLING_SPEED_INCR = 200f;
	private const float eps = FALLING_SPEED_MAX / FALLING_SPEED_INCR;

	private enum FireballState { Falling, Fallen }
	private FireballState state;

	public GameObject ball;

	void Start() {
		fallingSpeed = Random.Range(FALLING_SPEED_MIN, FALLING_SPEED_MAX) / FALLING_SPEED_INCR;
		state = FireballState.Falling;

		base.Start();
	}

	// Update is called once per frame
	void Update() {
		base.Update();

		if (state == FireballState.Fallen) return;

		if (Mathf.Abs(ball.transform.position.y - transform.position.y) > eps) {
			Vector3 newPosition = ball.transform.position;
			newPosition.y -= fallingSpeed;
			ball.transform.position = newPosition;
		} else {
			ball.transform.position = transform.position;

			state = FireballState.Fallen;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (state != FireballState.Fallen) return;

		if (col.gameObject.CompareTag("Player")) {
			PlayerGridObject player = col.GetComponentInParent<PlayerGridObject>();
			player.TakeDamage(5);

			Destroy(gameObject);
		}
	}
}

