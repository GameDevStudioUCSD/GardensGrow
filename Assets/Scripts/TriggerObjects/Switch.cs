    using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Switch : KillableGridObject {

    public UnityEvent pressEvent;
    public UnityEvent unpressEvent;
    public bool timed = false;
    public int timeout = 0;

    public Sprite switchUnpowered;
    public Sprite switchPowered;

    private int timer = 0;
    private bool pressed;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        pressed = false;
        //animator = this.gameObject.GetComponent<Animator>();
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    protected override void Update() {
        base.Update();
        if (pressed && timed) {
            timer--;
            if (timer <= 0) {
                unpressEvent.Invoke();
				renderer.sprite = switchUnpowered;
                pressed = false;
            }
        }
    }

    public override bool TakeDamage(int damage) {
        if (!pressed) {
            pressEvent.Invoke();
            renderer.sprite = switchPowered;
            pressed = true;
            if (timed) timer = timeout;
        }
        else if (timed) {
            timer = timeout;
        }
        else {
            unpressEvent.Invoke();
            renderer.sprite = switchUnpowered;
            pressed = false;
        }
        return false;
    }

    public override bool TakeBombDamage(int damage) {
        return TakeDamage(damage);
    }
}
