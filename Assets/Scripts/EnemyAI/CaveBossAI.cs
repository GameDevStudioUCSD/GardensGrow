using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveBossAI : KillableGridObject {

    private enum State {
        MOVING,
        ATTACKING,
        STUNNED,
        ANGRY,
    }

    public List<Transform> junctions;

    public GameObject emblem;

    public int stunDuration;
    public int stunImmuneDuration;
    public int attackDuration;

    public float speed;

    private int timeRemaining;
    private int toJunction;
    private State state;
    private int prevHealth;

    private Animator anim;

    protected override void Start() {
        base.Start();

        state = State.MOVING;
        toJunction = GetNewTargetJunction();
        prevHealth = health;

        anim = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();

        if (timeRemaining > 0) {
            --timeRemaining;
        }

        Vector3 dt;

        switch (state) {
            case State.MOVING:
                if (Vector2.Distance(transform.position, junctions[toJunction].transform.position) <= speed * 2) {
                    toJunction = GetNewTargetJunction();
                    state = State.ATTACKING;
                    timeRemaining = attackDuration;
                }

                dt = (junctions[toJunction].position - transform.position).normalized * speed;
                dt /= Globals.pixelSize;
                dt = new Vector3(Mathf.Round(dt.x), Mathf.Round(dt.y), 0);
                dt *= Globals.pixelSize;
                transform.position += dt;
                break;

            case State.ATTACKING:
                if (timeRemaining <= 0) {
                    state = State.MOVING;
                }
                break;

            case State.STUNNED:
                if (health != prevHealth || timeRemaining <= 0) {
                    prevHealth = health;
                    anim.SetTrigger("Hurt");
                    state = State.ANGRY;
                    timeRemaining = stunImmuneDuration;
                }
                break;

            case State.ANGRY:
                if (Vector2.Distance(transform.position, junctions[toJunction].transform.position) <= speed * 2) {
                    toJunction = GetNewTargetJunction();
                }

                dt = (junctions[toJunction].transform.position - transform.position).normalized * speed * 2;
                dt /= Globals.pixelSize;
                dt = new Vector3(Mathf.Round(dt.x), Mathf.Round(dt.y), 0);
                dt *= Globals.pixelSize;
                transform.position += dt;

                if (timeRemaining <= 0) {
                    state = State.MOVING;
                }
                break;

            default:
                Debug.LogError("CaveBossAI: Unknown state");
                break;
        }
    }

    public override bool TakeDamage(int damage) {
        if (state != State.STUNNED) {
            return false;
        }

        base.health -= damage;

        if (health <= 0) {
            emblem.SetActive(true);
            anim.SetTrigger("Death");
            Die();
        }

        return true;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Junction")) {
            CircuitJunction junction = collision.gameObject.GetComponent<CircuitJunction>();
            if (junction != null) {
                junction.system.ConnectJunction();
            }
        }
        else if (collision.gameObject.GetComponent<Boomerang>() != null) {
            if (state != State.ANGRY) {
                state = State.STUNNED;
                timeRemaining = stunDuration;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerGridObject player = collision.GetComponent<PlayerGridObject>();
            if (player != null) {
                player.TakeDamage(base.damage);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Junction")) {
            CircuitJunction junction = collision.gameObject.GetComponent<CircuitJunction>();
            junction.system.DisconnectJunction();
        }
    }

    private int GetNewTargetJunction() {
        int nextJunction;
        do {
            nextJunction = Mathf.FloorToInt(junctions.Count * Random.Range(0.0f, 0.999999999f));
        }
        while (nextJunction == toJunction);
        return nextJunction;
    }
}
