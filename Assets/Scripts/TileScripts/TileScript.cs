using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    [Header("Gizmo Settings")]
    [Tooltip("What color should this gizmo be?")]
    public Color gizmoColor = Color.yellow;
    public bool shouldDrawGizmos = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if (shouldDrawGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}
