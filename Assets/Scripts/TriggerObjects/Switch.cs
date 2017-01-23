using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Switch : KillableGridObject {

    public UnityEvent pressEvent;
    public UnityEvent unpressEvent;
    private bool pressed;
    private Animator animator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pressed = false;
        //animator = this.gameObject.GetComponent<Animator>();
    }

    public override bool TakeDamage(int damage)
    {
        if (!pressed)
        {
            pressEvent.Invoke();
            pressed = true;
        }
        else
        {
            unpressEvent.Invoke();
            pressed = false;
        }
        return false;
    }
}
