using UnityEngine;
using System.Collections;

public class LightPlantObject : PlantGridObject {

    public int radius;
    public float lightLevel;

    void OnEnable() {
    	for (int i = 1; i <= radius; i += 1) {
        	Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, i);
        	foreach (Collider2D collider in colliders) {
            	LightLevel ll = collider.GetComponent<LightLevel>();
           	 	if (ll != null) {
                    ll.ChangeLightLevel(1.0f / lightLevel);
                }
        	}
        }
    }

    void OnDisable() {
        for (int i = 1; i <= radius; i += 1) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, i);
            foreach (Collider2D collider in colliders) {
                LightLevel ll = collider.GetComponent<LightLevel>();
                if (ll != null) {
                    ll.ChangeLightLevel(-1.0f / lightLevel);
                }
            }
        }
    }
}
