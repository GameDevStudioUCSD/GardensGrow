using UnityEngine;
using System.Collections;

public class Tentacle : KillableGridObject {

    public float speed = 1.0f;
    public int damage = 1;
    public int tentacleNum;
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


        if (boss.hp > 0)
        {
            if ((other.gameObject.GetComponent<PlantProjectileObject>() && tentacleNum == 0) || //watermelon
                (other.gameObject.GetComponent<TurbinePlantObject>() && tentacleNum == 1) || //turbine
                (other.gameObject.GetComponent<CactusPlantObject>() && tentacleNum == 2) || //cactus
                ((other.gameObject.GetComponent<BombPlantObject>() || other.gameObject.GetComponent<BombObject>()) && tentacleNum == 3) || //bomb
                (other.gameObject.GetComponent<LightPlantObject>() && tentacleNum == 4) || //mushroom
                ((other.gameObject.GetComponent<BoomerangPlantObject>() || other.gameObject.GetComponent<Boomerang>()) && tentacleNum == 5) ||//boomerang
                (other.gameObject.GetComponent<SpinningPlant>() && tentacleNum == 6))
            {
                //play damaged animation
                boss.touchedPlayer = true; //makes tentacles retract
                boss.hp--;

                //do something when boss.hp == 0
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
