using UnityEngine;

public class Tile : MonoBehaviour {

    [Header("Gizmo Settings")]
    [Tooltip("What color should this gizmo be?")]
    public Color gizmoColor = Color.yellow;
    public bool shouldDrawGizmos = true;

    [ContextMenuItem("Set Patheable", "SetPatheable")]
    public bool isPatheable;

    // Path cost 
    // How costly it is to go through this tile
    // (e.x. 1 for normal, 2 for swamp, 5 for mountain)
    public int gCost = 1;

	// Use this for initialization
	/*void Start () {
	}*/
	
	// Update is called once per frame
	// Commented out for performance improvement
	/*void Update () {
	
	}*/

    void OnDrawGizmos()
    {
        if (shouldDrawGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D other) {

        // Check if there is a terrain object that is blocking the path on this tile
        TerrainObject terrainObj = other.GetComponent<TerrainObject>();
        if(terrainObj)
        {
            // If the terrain object is a barrier, then this tile is not patheable
            isPatheable = !terrainObj.isBarrier;
        }

    }
    */

    /// <summary>
    /// Automatically detects if there is a barrier terrain object on this tile
    /// setting the isPatheable field accordingly.
    /// 
    /// To use right-click isPatheable field and select "Set Patheable"
    /// </summary>
    private void SetPatheable()
    {
        bool cleanUpCollider = false; // Checks to see if we should remove temporarily created collider
        Collider2D tileCollider = GetComponent<Collider2D>();
        if(tileCollider == null)
        {
            // Create a temporary collider to raycast from
            cleanUpCollider = true;
            tileCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        RaycastHit2D[] results = new RaycastHit2D[5];

        // Create raycast to see if there are any barriers on this tile
        int numRayCollisions = tileCollider.Raycast(Vector2.right, results, 0.49f);

        //Debug.Log("Num col: " + numRayCollisions);

        isPatheable = true;

        // If there were any collision, then check if colliding object is a terrain barrier
        if(numRayCollisions > 0)
        {
            for(int i = 0; i < numRayCollisions; i++)
            {
                TerrainObject terrainObj = results[i].collider.GetComponent<TerrainObject>();
                //Debug.Log(results[i].collider.gameObject.name);
                if(terrainObj)
                {
                    if(terrainObj.isBarrier)
                    {
                        isPatheable = false;
                        break;
                    }
                }
            }
        }

        if(cleanUpCollider)
        {
            // Clean up temporary collider
            DestroyImmediate(tileCollider);
        }
    }

    protected void OnEnable() {
        SetPatheable();
    }
}
