using UnityEngine;
using System.Collections;

public class TurbinePlantObject : PlantGridObject
{
    //windSlime break into smaller ones variables
    public GameObject littleSlime;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation = Quaternion.identity;
    private bool canKillWindSlime = true;

    public float speed;
    private float moveNum;
    private MoveableGridObject enemy;

    public Collider2D southCollider;
    public Collider2D northCollider;
    public Collider2D eastCollider;
    public Collider2D westCollider;

    private Collider2D directionalCollider;

	private Animator animator;
    private int counter = 0; 
    // Use this for initialization
    protected override void Start()
    {
		// setting direction for corresponding animation
		animator = GetComponent <Animator> ();
        setDirection();

        base.Start();
    }

    // Update is called once per frame
	protected override void Update()
    {
        //the following if fixed a bug, plz don't remove it
        counter++;
        if (counter == 50)
        {
            setDirection();
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        base.Update();
	}

	public void setDirection ()
	{
		switch (this.direction) {
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
		animator.SetInteger ("Direction", (int)direction);	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        MoveableGridObject otherGridObject = other.GetComponent<MoveableGridObject>();
        BombObject bombObject = other.GetComponent<BombObject>();
        if (bombObject) {
            bombObject.Roll(direction);
        }
        if (other.gameObject.CompareTag("Player")) {
        	PlayerGridObject player = other.GetComponent<PlayerGridObject>();
        	if (!player.onPlatform) {
        		player.Move(direction);
        	}
        }
        else if (otherGridObject && !other.gameObject.CompareTag("WindSlime"))
        {
           	otherGridObject.Move(direction);
           	EnemyGridObject enemyGridObject = other.gameObject.GetComponent<EnemyGridObject>();
            if (enemyGridObject)
            {
            	enemyGridObject.TakeDamage(damage);
            }
        }
    }
    IEnumerator killWindSlimeCD(Collider2D other)
    {
        canKillWindSlime = false;
        spawnPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, 0.0f);
        GameObject newLilSlime = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
        other.gameObject.GetComponent<WindSlime>().list.Add(newLilSlime);
        newLilSlime.GetComponent<littleWindSlime>().tileMap = other.gameObject.GetComponent<WindSlime>().tileMap;
        newLilSlime.GetComponent<littleWindSlime>().targetObject = other.gameObject.GetComponent<WindSlime>().targetObject;

        spawnPosition = new Vector3(other.gameObject.transform.position.x + 1, other.gameObject.transform.position.y, 0.0f);
        GameObject newLilSlime2 = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
        other.gameObject.GetComponent<WindSlime>().list.Add(newLilSlime2);
        newLilSlime2.GetComponent<littleWindSlime>().tileMap = other.gameObject.GetComponent<WindSlime>().tileMap;
        newLilSlime2.GetComponent<littleWindSlime>().targetObject = other.gameObject.GetComponent<WindSlime>().targetObject;

        spawnPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y - 1, 0.0f);
        GameObject newLilSlime3 = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
        other.gameObject.GetComponent<WindSlime>().list.Add(newLilSlime3);
        newLilSlime3.GetComponent<littleWindSlime>().tileMap = other.gameObject.GetComponent<WindSlime>().tileMap;
        newLilSlime3.GetComponent<littleWindSlime>().targetObject = other.gameObject.GetComponent<WindSlime>().targetObject;

        spawnPosition = new Vector3(other.gameObject.transform.position.x - 1, other.gameObject.transform.position.y, 0.0f);
        GameObject newLilSlime4 = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
        other.gameObject.GetComponent<WindSlime>().list.Add(newLilSlime4);
        newLilSlime4.GetComponent<littleWindSlime>().tileMap = other.gameObject.GetComponent<WindSlime>().tileMap;
        newLilSlime4.GetComponent<littleWindSlime>().targetObject = other.gameObject.GetComponent<WindSlime>().targetObject;
     
        Destroy(other.gameObject);

        yield return new WaitForSeconds(1.0f);
        canKillWindSlime = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<WindSlime>() && canKillWindSlime)
        {
            StartCoroutine(killWindSlimeCD(other));
        }
    }
}
