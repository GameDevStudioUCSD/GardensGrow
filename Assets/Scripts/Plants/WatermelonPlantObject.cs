using UnityEngine;
using System.Collections;

public class WatermelonPlantObject : PlantGridObject
{

    public SeedProjectileObject seed;
    private int counter;
    public int shotDelay;

    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;
    private Animator animator;

    private Collider2D directionalCollider;

    // Use this for initialization
    void Start()
    {
        counter = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (counter > shotDelay)
        {
            Shooter();
            counter = 0;
        }
        counter++;
    }
    private void Shooter()
    {
        if (northCollider.enabled)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else if (westCollider.enabled)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else if (eastCollider.enabled)
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
        else
        {
            Vector3 spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(seed, spawnPosition, spawnRotation);
        }
    }
    void setDirection()
    {
        switch (this.direction)
        {
            case Globals.Direction.North:
                southCollider.enabled = false;
                eastCollider.enabled = false;
                northCollider.enabled = true;
                westCollider.enabled = false;
                directionalCollider = northCollider;
                break;
            case Globals.Direction.South:
                southCollider.enabled = true;
                eastCollider.enabled = false;
                northCollider.enabled = false;
                westCollider.enabled = false;
                directionalCollider = southCollider;
                break;
            case Globals.Direction.East:
                southCollider.enabled = false;
                eastCollider.enabled = true;
                northCollider.enabled = false;
                westCollider.enabled = false;
                directionalCollider = eastCollider;
                break;
            case Globals.Direction.West:
                southCollider.enabled = false;
                eastCollider.enabled = false;
                northCollider.enabled = false;
                westCollider.enabled = true;
                directionalCollider = westCollider;
                break;
        }
        animator.SetInteger("Direction", (int)direction);
    }
}