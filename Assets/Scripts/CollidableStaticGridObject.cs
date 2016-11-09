using UnityEngine;
using System.Collections;

public class CollidableStaticGridObject : StaticGridObject, Collidable
{

    private BoxCollider2D boxCollider;

    private Tile tile;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Collidable other = collision.gameObject.GetComponent<Collidable>();

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //Collidable other = collision.gameObject.GetComponent<Collidable>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //Collidable other = collision.gameObject.GetComponent<Collidable>();

    }

}