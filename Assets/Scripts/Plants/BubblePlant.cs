using UnityEngine;
using System.Collections;

public class BubblePlant : PlantGridObject {

    public GameObject bubble;
    public float bubbleCooldownFrames;
    private GameObject currentBubble = null;
    private float bubbleTimer;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        currentBubble = (GameObject)Instantiate(bubble, transform.position, Quaternion.identity);
        bubbleTimer = bubbleCooldownFrames;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	    if (!currentBubble) {
            bubbleTimer -= 1;
            if (bubbleTimer <= 0) {
                currentBubble = (GameObject)Instantiate(bubble, transform.position, Quaternion.identity);
                bubbleTimer = bubbleCooldownFrames;
            }
        }
	}
}
