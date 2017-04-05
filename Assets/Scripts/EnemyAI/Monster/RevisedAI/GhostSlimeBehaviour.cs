using UnityEngine;
using System.Collections;

public class GhostSlimeBehaviour : GenericMonsterBehaviour {

    protected override void Start() {
        base.Start();
        isInvulnerable = true;
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
