using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogID : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((int)Time.time % 3 == 1) 
			Debug.Log (GetComponentInChildren<Text> ().GetInstanceID());
	}
}
