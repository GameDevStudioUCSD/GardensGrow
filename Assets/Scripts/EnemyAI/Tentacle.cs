using UnityEngine;
using System.Collections;

public class Tentacle : KillableGridObject {

    public float speed = 1.0f;
    public int damage = 1;
    public int tentacleNum;

    
    //evil plants
    public GameObject evilWatermelonPlant;
    public GameObject evilTurbinePlant;
    public GameObject evilCactusPlant;
    public GameObject evilBoomerangPlant;
    public GameObject evilBombPlant;
    public GameObject evilLightPlant;
    public GameObject evilSpinningPlant;

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


        if (boss.hp == 0)
        {
            //instantiate boss weed seed
        }

        if (other.gameObject.GetComponent<PlantGridObject>() && boss.hp != 0)
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
            }
            else
            {

                spawnPosition = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, 0f);
                if (other.gameObject.GetComponent<WatermelonPlantObject>())
                {
                    if (!other.gameObject.GetComponent<WatermelonPlantObject>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilWatermelonPlant, spawnPosition, spawnRotation);
                    }
                }
                if (other.gameObject.GetComponent<TurbinePlantObject>())
                {
                    if (!other.gameObject.GetComponent<TurbinePlantObject>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilTurbinePlant, spawnPosition, spawnRotation);
                    }
                }
                if (other.gameObject.GetComponent<CactusPlantObject>())
                {
                    if (!other.gameObject.GetComponent<CactusPlantObject>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilCactusPlant, spawnPosition, spawnRotation);
                    }
                }
                if(other.gameObject.GetComponent<BoomerangPlantObject>())
                {
                    if (!other.gameObject.GetComponent<BoomerangPlantObject>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilBoomerangPlant, spawnPosition, spawnRotation);
                    }
                }
                if(other.gameObject.GetComponent<BombPlantObject>())
                {
                    if (!other.gameObject.GetComponent<BombPlantObject>().evil)
                    {
                        if (other.gameObject.GetComponent<BombPlantObject>().bomb)
                        {
                            Destroy(other.gameObject.GetComponent<BombPlantObject>().bomb.gameObject);
                        }
                        Destroy(other.gameObject);
                        Instantiate(evilBombPlant, spawnPosition, spawnRotation);
                    }
                    else
                    {
                        BombObject temp = other.gameObject.GetComponent<BombPlantObject>().bomb;
                        if (temp)
                        {
                            temp.Roll(this.direction);
                        }
                    }
                }
                if (other.gameObject.GetComponent<LightPlantObject>())
                {
                    if (!other.gameObject.GetComponent<LightPlantObject>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilLightPlant, spawnPosition, spawnRotation);
                    }
                    else
                    {
                        CircuitSystem cs = FindObjectOfType<CircuitSystem>();
                        cs.isLit = true;
                        cs.ConnectJunction();
                    }
                }
                if (other.gameObject.GetComponent<SpinningPlant>())
                {
                    if (!other.gameObject.GetComponent<SpinningPlant>().evil)
                    {
                        Destroy(other.gameObject);
                        Instantiate(evilSpinningPlant, spawnPosition, spawnRotation);
                    }
                }
            }
            
        }
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
