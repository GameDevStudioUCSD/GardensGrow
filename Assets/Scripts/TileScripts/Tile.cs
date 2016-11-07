using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Each tile corresponds to a square in the grid and can contain up to 1 CollidableStaticGridObject at a time (ex. wall, plant) and any number of UncollidableStaticGridObject (ex.conveyor belt).
/// Each MovableGridObject on this tile is given all applicable effects of all UncollidableStaticGridObjects in this tile.  
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// The singular collidable object, such as wall or plant.
    /// </summary>
    public CollidableStaticGridObject collidableStaticGridObject;

    /// <summary>
    /// Any number of uncollidable grid objects, such as conveyor belts, that affect moveable objects that happend to be on this tile.
    /// </summary>
    public LinkedList<UncollidableStaticGridObject> uncollidableStaticGridObjects;



    [Header("Gizmo Settings")]
    [Tooltip("What color should this gizmo be?")]
    public Color gizmoColor = Color.yellow;
    public bool shouldDrawGizmos = true;


    // Path cost 
    // How costly it is to go through this tile
    // (e.x. 1 for normal, 2 for swamp, 5 for mountain)
    public int gCost = 1;

	// Use this for initialization
	void Awake () {
        uncollidableStaticGridObjects = new LinkedList<UncollidableStaticGridObject>();
    }

    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ApplyEffects(MoveableGridObject g)
    {
        foreach (UncollidableStaticGridObject s in uncollidableStaticGridObjects)
        {
            s.ApplyEffect(g);
        }
    }

    public void ClearEffects(MoveableGridObject g)
    {
        foreach (UncollidableStaticGridObject s in uncollidableStaticGridObjects)
        {
            s.ClearEffect(g);
        }
    }

    public void AddUncollidableStaticGridObject(UncollidableStaticGridObject obj)
    {
        uncollidableStaticGridObjects.AddLast(obj);
    }

    public void RemoveUncollidableStaticGridObject(UncollidableStaticGridObject obj)
    {
        uncollidableStaticGridObjects.Remove(obj);
    }

    public bool isPathable()
    {
        return collidableStaticGridObject == null;
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
