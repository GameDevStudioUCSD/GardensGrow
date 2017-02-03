using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Switch : KillableGridObject {

    public UnityEvent pressEvent;
    public UnityEvent unpressEvent;
    public bool timed = false;
    public int timeout = 0;
    private int timer = 0;
    private bool pressed;
    private Animator animator;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        pressed = false;
        //animator = this.gameObject.GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();
        if (pressed && timed) {
            timer--;
            if (timer <= 0) {
                unpressEvent.Invoke();
                pressed = false;
            }
        }
    }

    public override bool TakeDamage(int damage) {
        if (!pressed) {
            pressEvent.Invoke();
            pressed = true;
            if (timed) timer = timeout;
        }
        else if (timed) {
            timer = timeout;
        }
        else {
            unpressEvent.Invoke();
            pressed = false;
        }
        return false;
    }
}
