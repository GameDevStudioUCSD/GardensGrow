using UnityEngine;
using System.Collections;

public class CollidableStaticGridObject : StaticGridObject, Collidable
{
    public float initialColliderWidth, initialColliderHeight;

    private BoxCollider2D boxCollider;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(initialColliderWidth, initialColliderHeight);
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