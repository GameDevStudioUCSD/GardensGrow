using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SpinSwitch : MonoBehaviour {

    public UnityEvent pressEvent;
    public UnityEvent unpressEvent;
    public bool timed = false;
    public int timeout = 0;

    public Sprite switchUnpowered;
    public Sprite switchPowered;

    private int timer = 0;
    private bool pressed;
    private new SpriteRenderer renderer;

    private SpinningPlant spinner;

    // Use this for initialization
    protected virtual void Start() {
        pressed = false;
        //animator = this.gameObject.GetComponent<Animator>();
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void Update() {
        if (pressed && !timed && !spinner) {
            unpressEvent.Invoke();
            renderer.sprite = switchUnpowered;
            pressed = false;
        }

        if (pressed && timed) {
            timer--;
            if (timer <= 0) {
                if (spinner) spinner.TakeDamage(100);
                unpressEvent.Invoke();
                renderer.sprite = switchUnpowered;
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
                renderer.sprite = switchPowered;
                pressed = true;
                if (timed) timer = timeout;
            }
            else if (timed) {
                timer = timeout;
            }
        }
    }
}
