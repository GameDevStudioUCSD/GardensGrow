using UnityEngine;
using System.Collections;

public class OxygenLevel : MonoBehaviour {

    public float oxygenFrames = 5400;
    public float currentOxygen;

    protected virtual void Start() {
        currentOxygen = oxygenFrames;
    }
	
	protected virtual void Update () {
        currentOxygen--;
        if (currentOxygen <= 0) Globals.player.TakeDamage(int.MaxValue);
	}

    public void RefillOxygen() {
        currentOxygen = oxygenFrames;
    }
}
