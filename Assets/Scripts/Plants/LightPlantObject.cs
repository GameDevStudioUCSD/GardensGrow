using UnityEngine;
using System.Collections;

public class LightPlantObject : PlantGridObject {

    [SerializeField]
    private int radius;

    [SerializeField]
    private int lightLevel;

    void OnEnable() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders) {
            LightLevel ll = collider.GetComponent<LightLevel>();
            if (ll != null) {
                ll.Brighten(lightLevel);
            }
        }
    }

    void OnDisable() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders) {
            LightLevel ll = collider.GetComponent<LightLevel>();
            if (ll != null) {
                ll.Dim(lightLevel);
            }
        }
    }
}
