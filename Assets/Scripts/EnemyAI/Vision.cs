using UnityEngine;
using System.Linq;
using System.Collections;

public class Vision : MonoBehaviour {

    [Header("Ocular Settings")]
    public float maximumViewDistance = 5;
    [Header("Debugging Settings")]
    public Vector3 lookDirection;
    public bool debugMode;
	
	// Update is called once per frame
	void Update () {
        if(debugMode)
        {
            Debug.DrawRay(transform.position, GetLookRay());
            Debug.Log(CanSeePlayer(Globals.Direction.North));
        }
    }

    /// <summary>
    /// Returns true if the player is in the line of sight
    /// </summary>
    /// <param name="direction">Direction enum in which to look</param>
    /// <returns>Boolean if the player is in line of sight</returns>
    public bool CanSeePlayer(Globals.Direction direction)
    {
        return CanSeePlayer(Globals.DirectionToVector(direction));
    }

    /// <summary>
    /// Vectorized implementation of can see player
    /// </summary>
    /// <param name="direction">The vector in which to look</param>
    /// <returns></returns>
    public bool CanSeePlayer(Vector3 direction)
    {
        // Assume we see nothing until we see it
        bool returnValue = false;
        // Consider everything we see
        foreach (GameObject o in LookInDirection(direction))
        {
            // If we see the player, then return true.
            if (o.GetComponent<PlayerGridObject>())
            {
                returnValue = true;
                break;
            }
        }
        return returnValue;
    }

    /// <summary>
    /// Returns all gameobjects within the line of sight
    /// </summary>
    /// <param name="direction">Direction enum in which to look</param>
    /// <returns>An array of gameobjects within the line of sight</returns>
    public GameObject[] LookInDirection(Globals.Direction direction)
    {
        return LookInDirection(Globals.DirectionToVector(direction));
    }

    /// <summary>
    /// Returns all gameobjects within the line of sight
    /// This implementation relies on vectors
    /// </summary>
    /// <param name="direction">The vector in which to look</param>
    /// <returns></returns>
    public GameObject[] LookInDirection( Vector3 direction )
    {
        lookDirection = direction.normalized;
        // First, get a list of all the raycast hits within the direction
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lookDirection, maximumViewDistance);
        // Linq allows for filter like behavior in C#.
        // This transforms the list of RaycastHit2Ds into a list of GameObjects
        var gameObjects = from hit in hits select hit.collider.gameObject;
        // Extract the array and return it
        return gameObjects.ToArray();
    }

    Vector3 GetLookRay()
    {
        return maximumViewDistance * lookDirection;
    }
}
