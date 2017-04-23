using UnityEngine;
using System.Collections;

public class GhostSlimeBehaviour : GenericMonsterBehaviour {

    private CircleCollider2D lightRadius;
    private int lightSources = 0;

    protected override void Start() {
        base.Start();
        isInvulnerable = true;
    }

    protected override void Update() {
        base.Update();
        if (lightSources > 0 && isInvulnerable) Lighten();
        else if (lightSources <= 0 && !isInvulnerable) Darken();
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        LightPlantObject lightPlant = other.GetComponent<LightPlantObject>();
        NaturalLight naturalLight = other.GetComponent<NaturalLight>();
        if (lightPlant || naturalLight) lightSources++;
    }

    protected void OnTriggerExit2D(Collider2D other) {
        LightPlantObject lightPlant = other.GetComponent<LightPlantObject>();
        NaturalLight naturalLight = other.GetComponent<NaturalLight>();
        if (lightPlant || naturalLight) lightSources--;
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
