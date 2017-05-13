using UnityEngine;
using System.Collections;

public class Tentacle : KillableGridObject {

    public float speed = 1.0f;
    public int damage = 1;
    public int tentacleNum;

    //evil plants
    public GameObject evilWatermelonPlant;

    //instantiating stuff
    private Quaternion spawnRotation = Quaternion.Euler(0, 0, 0f);
    private Vector3 spawnPosition;

    FinalDungeonBoss boss;

    /* Tentacle nums
     * 0    watermelon  red
     * 1    turbine     green
     * 2    cactus      yellow
     * 3    bomb        pink
     * 4    mushroom    black
     * 5    boomerang   blue
     * 6    spinning    teal
     */
    void Start()
    {
        boss = FindObjectOfType<FinalDungeonBoss>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        /*tentacle attack player*/
        if (other.gameObject.GetComponent<PlayerGridObject>())
        {
            other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(1);
            boss.touchedPlayer = true;
        }

        /*player plants attack tentacle*/

        if (other.gameObject.GetComponent<PlantGridObject>())
        {
            if ((other.gameObject.GetComponent<PlantProjectileObject>() && tentacleNum == 0) || //watermelon
                (other.gameObject.GetComponent<TurbinePlantObject>() && tentacleNum == 1) || //turbine
                (other.gameObject.GetComponent<CactusPlantObject>() && tentacleNum == 2) || //cactus
                ((other.gameObject.GetComponent<BombPlantObject>() || other.gameObject.GetComponent<BombObject>()) && tentacleNum == 3) || //bomb
                (other.gameObject.GetComponent<LightPlantObject>() && tentacleNum == 4) || //mushroom
                ((other.gameObject.GetComponent<BoomerangPlantObject>() || other.gameObject.GetComponent<Boomerang>()) && tentacleNum == 5) ||//boomerang
                (other.gameObject.GetComponent<SpinningPlant>() && tentacleNum == 6)) //spinning
            {
                //play damaged animation
                boss.touchedPlayer = true; //makes tentacles retract
                boss.hp--;

                if(boss.hp == 0)
                {    
                    //play spawning boss sound
                    //spawn boss to deplant in the middle of the screen
                }
            }
            else
            {
                //make other.gameObject.GetComponent<PlantGridObject>() evil then turn into a seed after x seconds
                if (other.gameObject.GetComponent<WatermelonPlantObject>())
                {
                    if(!other.gameObject.GetComponent<WatermelonPlantObject>().evil)
                    spawnPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, 0f);
                    Destroy(other.gameObject);
                    StartCoroutine(spawnEvilPlant(evilWatermelonPlant));
                }
            }
            
        }
    }
    IEnumerator spawnEvilPlant(GameObject other)
    {
        GameObject newEvilPlant = (GameObject) Instantiate(other, spawnPosition, spawnRotation);
        yield return new WaitForSeconds(5.0f); //leaves evil plant spawnned for 5 seconds
        Destroy(newEvilPlant.gameObject); //for some reason this doesn't work to destory evilPlant

    }
    public void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize*speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize*speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize * speed;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize * speed;
            this.transform.position = position;
        }
    }
}
