using UnityEngine;
using System.Collections;

public class CollidableStaticGridObject : StaticGridObject, Collidable
{
    public float initialColliderWidth, initialColliderHeight;

    private BoxCollider2D boxCollider;

    private Tile tile;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(initialColliderWidth, initialColliderHeight);

        tile = GameObject.Find("TileMap").GetComponent<TileMap>().GetTileStandingOn(gameObject.transform.position);
        if (tile != null)
        {
            tile.SetCollidableStaticGridObject(this);
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public float GetInitialColliderWidth()
    {
        return initialColliderWidth;
    }

    public float GetInitialColliderHeight()
    {
        return initialColliderHeight;
    }
}