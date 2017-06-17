using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummoningModule : MonoBehaviour {

    public SummoningParameters parameters;

    protected List<GameObject> summons;
    protected float spawnCooldownTimer = 0.0f;
    // Ready to spawn based on cooldown timer
    protected bool spawnTimeReady;

    public void Start()
    {
        spawnTimeReady = false;
        summons = new List<GameObject>(parameters.maxNumberOfSpawns);
    }

    public void Update()
    {
        if(!spawnTimeReady)
        {
            spawnCooldownTimer += Time.deltaTime;
            if(spawnCooldownTimer > parameters.spawnCooldown)
            {
                spawnTimeReady = true;
                spawnCooldownTimer = 0.0f;
            }
        }
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

        GameObject summonedMonster = (GameObject)Instantiate(parameters.summonedGameObject, spawnPosition, Quaternion.identity);

        // Set tilemap and target in summoned monster
        PathFindingModule monsterPathFinding = summonedMonster.GetComponentInChildren<PathFindingModule>();
        monsterPathFinding.parameters.tileMap = parameters.tileMap;
        monsterPathFinding.parameters.target = parameters.target;

        summons.Add(summonedMonster);

        spawnTimeReady = false;

        // Activate monster
        summonedMonster.GetComponent<MonsterBehaviourAbstractFSM>().StartAI();
    }

    public bool CanSummon()
    {
        bool canSummon = false;

        // Remove any dead summons from list
        // Iterate backwards to avoid any list removal issues
        for(int i = summons.Count - 1; i >= 0; i--)
        {
            if (summons[i] == null)
                summons.RemoveAt(i);
        }

        // Is there an open spot for a summon
        if (summons.Count < parameters.maxNumberOfSpawns)
            canSummon = true;

        // Is there an open spot and is the summon off cooldown
        return canSummon && spawnTimeReady;
    }

    [Serializable]
    public class SummoningParameters
    {
        [Header("Summoning Components")]
        public GameObject summonedGameObject;
        [Range(0, 99)]
        public int maxNumberOfSpawns;
        [Range(0.0f, 3.0f)] //controls max spawns at once
        public float spawnCooldown;
        public TileMap tileMap;
        public GameObject target;
    }
}
