using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SpinSwitch : MonoBehaviour {

    public UnityEvent pressEvent;
    public UnityEvent unpressEvent;
    public bool timed = false;
    public int timeout = 0;

    private int timer = 0;
    private bool pressed;

    private SpinningPlant spinner;
    private Animator animator;

    // Use this for initialization
    protected virtual void Start() {
        pressed = false;
        animator = this.gameObject.GetComponent<Animator>();
    }

    protected virtual void Update() {
        if (pressed && !timed && !spinner) {
            unpressEvent.Invoke();
            pressed = false;
        }

        if (pressed && timed) {
            timer--;
            if (timer <= 0) {
                if (spinner) spinner.TakeDamage(100);
                unpressEvent.Invoke();
                pressed = false;
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        SpinningPlant spinningPlant = other.gameObject.GetComponent<SpinningPlant>();
        if (spinningPlant) {
            spinner = spinningPlant;
            if (!pressed) {
                pressEvent.Invoke();
                pressed = true;
                animator.SetTrigger("Trigger");
                if (timed) timer = timeout;
            }
            else if (timed) {
                timer = timeout;
            }
        }
    }
}
