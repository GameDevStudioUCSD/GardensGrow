using UnityEngine;
using System.Collections;

public class spinThePlayer : MonoBehaviour {

    private PlayerGridObject player;
    public int spinning;
    private float spin;
    private Quaternion spawnRotation = Quaternion.identity;

    private int counter = 0;
    public int turnDelay;
    private int maxSpinCounter = 0;
    public int maxSpin;
    private bool canSpin = false;

    void Start()
    {
        player = FindObjectOfType<PlayerGridObject>();
        spin = (float)spinning / 4;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.canMove = false;
            StartCoroutine(StopPlayer());
        }
    }
    void Update()
    {
        if (canSpin)
        {
            counter++;
            if (counter > turnDelay)
            {
                //player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
                player.direction = Globals.Direction.East;
                counter = 0;
                maxSpinCounter++;
                if (maxSpinCounter >= maxSpin)
                {
                    canSpin = false;
                    player.canMove = true;
                }
            }
        }
    }
    IEnumerator StopPlayer()
    {
        if(player.canMove == false)
        {
            //StartCoroutine(SpinPlayer());
            canSpin = true;
            yield return new WaitForSeconds(spinning);
        }
    }
    //don't use for now
    IEnumerator SpinPlayer()
    {
        player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
        yield return new WaitForSeconds(spin);
        player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
        yield return new WaitForSeconds(spin);
        player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
        yield return new WaitForSeconds(spin);
        player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
        yield return new WaitForSeconds(spin);
    }
}
