using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public UnityEngine.UI.Image[] uiPlants;
	public Sprite[] seedPackets;

	// Use this for initialization
	void Start () {
		UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void UpdateUI () {
		for (int i = 0; i < 8; i++)
		{
			if (Globals.unlockedSeeds[i] == true)
			{
				uiPlants[i].sprite = seedPackets[i];
			}
		}
	}
}
