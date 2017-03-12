using UnityEngine;
using System.Collections;

public class CircuitWire : MonoBehaviour {
    public int damage = 3;
    public Sprite unlitSprite;
    public Sprite halfLitSprite;
    public Sprite litSprite;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;

    void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        Unelectrify();
    }

    //deal damage; only works when colliders are enabled
    public void OnTriggerEnter2D(Collider2D other) {
        KillableGridObject killable = other.GetComponent<KillableGridObject>();
        if (killable) {
            killable.TakeDamage(damage);
        }
    }

    //changes current sprite
    public void SetFrame(int frame) {
        if (frame == 0) spriteRenderer.sprite = unlitSprite;
        else if (frame == 1) spriteRenderer.sprite = halfLitSprite;
        else spriteRenderer.sprite = litSprite;
    }

    //enables colliders so wires become dangerous
    public void Electrify() {
        foreach (Collider2D collider in colliders) {
            collider.enabled = true;
        }
    }

    //disables colliders so wires stop being dangerous
    public void Unelectrify() {
        foreach (Collider2D collider in colliders) {
            collider.enabled = false;
        }
    }
}
