using UnityEngine;
using System.Collections;

public class NaturalLight : StaticGridObject {

	public int radius;

    public float lightLevel;

    void OnEnable() {
    	for (int i = radius - 3 + 1; i <= radius; i++) {
        	Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, i);
        	foreach (Collider2D collider in colliders) {
            	LightLevel ll = collider.GetComponent<LightLevel>();
           	 	if (ll != null) {
            	    ll.Brighten(1 / lightLevel);
            	}
        	}
        }
    }

    void OnDisable() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders) {
            LightLevel ll = collider.GetComponent<LightLevel>();
            if (ll != null) {
                ll.Dim(1 / lightLevel);
            }
        }
    }
}
