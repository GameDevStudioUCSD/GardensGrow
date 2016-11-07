using UnityEngine;
using System.Collections;

/// <summary>
/// This is the superclass for any static grid object that does not collide with the player or other units.
/// </summary>
public abstract class UncollidableStaticGridObject : StaticGridObject
{
    private Tile tile;

    public override void Start()
    {
        base.Start();
        //Assigns this object to a tile in the map, if it is in a tile location.
        tile = GameObject.Find("TileMap").GetComponent<TileMap>().GetTile(gameObject.transform.position);
        if (tile != null)
        {
            tile.AddUncollidableStaticGridObject(this);
        }
    }

    /// <summary>
    /// For each movable object that is on top of the tile this static object is on, this function is called with that movable object as parameter.
    /// Check what type of object it is using instanceof and apply effects as needed.
    /// </summary>
    /// <param name="obj"></param>
    public abstract void ApplyEffect(MoveableGridObject obj);

    /// <summary>
    /// For each movable object, undo the change if necessary.
    /// </summary>
    /// <param name="obj"></param>
    public abstract void ClearEffect(MoveableGridObject obj);

    public override void OnDestroy()
    {
        //tile.RemoveUncollidableStaticGridObject(this);
        base.OnDestroy();
    }

}
