using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGridObject : MoveableGridObject {

    public int moveDistance;
    public int moveSpeed;
    private List<GameObject> moveList = new List<GameObject>();

	// Use this for initialization
	void Start () {

        moveList.Add(this.gameObject);
        Debug.Log("added platform to moveList");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Turbine"))
        {
            StartCoroutine(Move());
            Debug.Log("starting coroutine");
                
        }
        else if (!col.gameObject.CompareTag("Platform") || !col.gameObject.CompareTag("Ground")){
            moveList.Add(col.gameObject);
            Debug.Log("added to moveList:" + col.gameObject);
        }
    }
    IEnumerator Move()
    {
        //must detect direction left or right
        for (int i = 0; i < moveDistance; i++)
        {
            foreach (GameObject thing in moveList)
            {
                Vector3 position = this.transform.position;
                position.y -= .03125f;
                this.transform.position = position;
            }
        }

        //moves the platform and everything by changing transform.position.y
        yield return new WaitForSeconds(1/moveSpeed);
    }
}
