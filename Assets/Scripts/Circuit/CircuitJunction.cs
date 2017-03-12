using UnityEngine;
using System.Collections;

public class CircuitJunction : MonoBehaviour {
    public CircuitSystem system; //set by the CircuitSystem
    private bool isLit = false;
    private LightPlantObject plant = null;
	
    //checks if the junction has lost the mushroom that was lighting it
	void Update () {
	  if (isLit && !plant) {
            isLit = false;
            system.DisconnectJunction();
        }
	}

    //detects new light plants to light the junction
    public void OnTriggerEnter2D(Collider2D other) {
        if (!plant) {
            LightPlantObject newPlant = other.GetComponent<LightPlantObject>();
            if (newPlant) {
                plant = newPlant;
                isLit = true;
                system.ConnectJunction();
            }
        }
    }
}
