using UnityEngine;
using UnityEngine.UI;

public class PlantGridObject : KillableGridObject
{
	// Used for saving plant's position in the data structure
	protected Vector3 startLocation;

	protected override void Start() {
		startLocation = this.transform.position;
		base.Start();
	}

	protected override void Update() {
		base.Update();
	}

	protected override void Die() {
		Globals.plants.Remove(new Globals.PlantData(startLocation, Application.loadedLevelName,0));
		base.Die();
        
	}
}
