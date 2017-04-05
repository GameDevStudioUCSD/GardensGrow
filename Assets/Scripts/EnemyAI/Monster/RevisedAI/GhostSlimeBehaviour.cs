using UnityEngine;
using System.Collections;

public class GhostSlimeBehaviour : GenericMonsterBehaviour {

    int lightTimer = 0;

    protected override void Start() {
        base.Start();
        animator.SetTrigger("Darken");
        isInvulnerable = true;
    }

	protected override void Update() {
        base.Update();
        lightTimer++;
        if (lightTimer >= 200) {
            lightTimer = 0;
            if (isInvulnerable) Lighten();
            else Darken();
        }
	}

    public override void Move(Globals.Direction direction) { }

    public void Lighten() {
        animator.SetTrigger("Lighten");
        isInvulnerable = false;
    }

    public void Darken() {
        animator.SetTrigger("Darken");
        isInvulnerable = true;
    }
}
