using UnityEngine;

public class Tile : MonoBehaviour {

    [Header("Gizmo Settings")]
    [Tooltip("What color should this gizmo be?")]
    public Color gizmoColor = Color.yellow;
    public bool shouldDrawGizmos = true;

    [ContextMenuItem("Get Collider", "GetCollider")]
    public Collider2D tileCollider;

    [ContextMenuItem("Set Patheable", "SetPatheable")]
    public bool isPatheable = true;

    // Path cost 
    // How costly it is to go through this tile
    // (e.x. 1 for normal, 2 for swamp, 5 for mountain)
    public int gCost = 1;

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

    void OnTriggerEnter2D(Collider2D other) {

        // Check if there is a terrain object that is blocking the path on this tile
        TerrainObject terrainObj = other.GetComponent<TerrainObject>();
        if(terrainObj)
        {
            // If the terrain object is a barrier, then this tile is not patheable
            isPatheable = !terrainObj.isBarrier;
        }

    }

    /// <summary>
    /// Automatically finds the Collider2D attached to this object and
    /// sets the collider field.
    /// 
    /// To use right-click collider field and select "Get Collider"
    /// </summary>
    private void GetCollider()
    {
        tileCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Automatically detects if there is a barrier terrain object on this tile
    /// setting the isPatheable field accordingly.
    /// 
    /// To use right-click isPatheable field and select "Set Patheable"
    /// </summary>
    private void SetPatheable()
    {
        RaycastHit2D[] results = new RaycastHit2D[1];

        // Create raycast to see if there are any barriers on this tile
        int numRayCollisions = tileCollider.Raycast(Vector2.up, results, 0.5f);

        // If there were any collision, then check if colliding object is a terrain barrier
        if(numRayCollisions > 0)
        {
            TerrainObject terrainObj = results[0].collider.GetComponent<TerrainObject>();
            if(terrainObj)
            {
                isPatheable = !terrainObj.isBarrier;
            }
        }


    }
}
