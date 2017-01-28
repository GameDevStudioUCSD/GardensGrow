using UnityEngine;
using UnityEngine.UI;

public class PlantGridObject : KillableGridObject
{
	protected override void Update() {
		base.Update();
	}

	protected override void Die() {
		Globals.plants.Remove(new Globals.PlantData(this.transform.position, Application.loadedLevelName));
		base.Die();
	}
}
