using UnityEngine;
using System.Collections.Generic;

public class GhostSlimeBehaviour : GenericMonsterBehaviour {

    private CircleCollider2D lightRadius;
    private List<LightSource> lightSources = new List<LightSource>();

    protected override void Start() {
        base.Start();
        isInvulnerable = true;
    }

    protected override void Update() {
        base.Update();

        foreach (LightSource light in lightSources) {
            if (!light) {
                lightSources.Remove(light);
                break;
            }
        }

        if (lightSources.Count > 0 && isInvulnerable) Lighten();
        else if (lightSources.Count <= 0 && !isInvulnerable) Darken();
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        LightSource light = other.GetComponent<LightSource>();
        if (light) lightSources.Add(light);
    }

    protected void OnTriggerExit2D(Collider2D other) {
        LightSource light = other.GetComponent<LightSource>();
        if (light) {
            lightSources.Remove(light);
        }
    }

    //Call this every time the Slime enters the light
    public void Lighten() {
        animator.SetTrigger("Lighten");
        isInvulnerable = false;
    }

    //Call this every time the Slime exits the light
    public void Darken() {
        animator.SetTrigger("Darken");
        isInvulnerable = true;
    }
}
