using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummoningBehaviour : MonoBehaviour {

    protected SummoningParameters p = null;

    protected List<GameObject> summons;
    protected float spawnCooldownTimer = 0.0f;
    // Ready to spawn based on cooldown timer
    protected bool spawnReady;

    public void Update()
    {
        // Null guard
        if (p == null)
            return;

        // TODO: 
        if(!spawnReady)
        {
            spawnCooldownTimer += Time.deltaTime;
            if(spawnCooldownTimer > p.spawnCooldown)
            {
                spawnReady = true;
                spawnCooldownTimer = 0.0f;
            }
        }
        
    }

    public void SetParameters(SummoningParameters p)
    {
        this.p = p;

        summons = new List<GameObject>(p.maxNumberOfSpawns);
    }

    public void Summon()
    {
        int randInt = UnityEngine.Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        if (randInt == 1)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
        }
        else if (randInt == 2)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
        }
        else if (randInt == 3)
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
        }
        else
        {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
        }

        GameObject summonedSlime = (GameObject)Instantiate(p.summonedGameObject, spawnPosition, Quaternion.identity);

        GenericMonsterBehaviour monsterBehaviour = summonedSlime.GetComponent<GenericMonsterBehaviour>();
        monsterBehaviour.pathFindingParameters.tileMap = p.tileMap;
        monsterBehaviour.pathFindingParameters.target = p.target;
    }

    public bool CanSummon()
    {

    }

    [Serializable]
    public class SummoningParameters
    {
        [Header("Summoning Components")]
        public GameObject summonedGameObject;
        [Range(0, 99)]
        public int maxNumberOfSpawns;
        [Range(0.0f, 60.0f)]
        public float spawnCooldown;
        public TileMap tileMap;
        public GameObject target;
    }
}
