using UnityEngine;
using System.Collections;

public class LightLevelInvulnerable : LightLevel {
    private KillableGridObject killable;
    
	public override void Start () {
        killable = GetComponent<KillableGridObject>();
        if (level <= 0) killable.isInvulnerable = true;
        base.Start();
    }

    public override void ChangeLightLevel(float amount) {
        if (level <= 0) killable.isInvulnerable = true;
        else killable.isInvulnerable = false;
        base.ChangeLightLevel(amount);
    }
}
