using UnityEngine;
using System.Collections;

public class SpinningPlant : PlantGridObject {

    public float rotationSpeed = 10;
    public int framesBetweenDamage = 60;

    //prevents plant from being killed by bombs caught up in it, which would always destroy it otherwise
    public override bool TakeBombDamage(int damage) {
        return false;
    }
}
