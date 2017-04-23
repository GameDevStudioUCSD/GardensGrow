using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	protected virtual void OnTriggerEnter2D(Collider2D other) {
        PlayerGridObject player = other.GetComponent<PlayerGridObject>();
        if (player) {
            OxygenLevel oxygen = other.GetComponent<OxygenLevel>();
            if (oxygen) oxygen.RefillOxygen();
            Destroy(gameObject);
        }
    }
}
