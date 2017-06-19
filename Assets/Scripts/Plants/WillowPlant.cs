using UnityEngine;
using System.Collections;

public class WillowPlant : PlantGridObject {

	private int counter;

	void FixedUpdate() {
        PlayerGridObject p = FindObjectOfType<PlayerGridObject>();

		counter++;
		if (counter >= 200) {
            Globals.startCredits = true;
            p.deathPanel.SetActive(true);
			Application.LoadLevel(1);
		}
	}
}
