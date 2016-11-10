using UnityEngine;
using System.Collections;

public class CanvasScaling : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(POTAspectUtility.multiplier);
        this.GetComponent<Canvas>().scaleFactor = POTAspectUtility.multiplier;
    }
}
