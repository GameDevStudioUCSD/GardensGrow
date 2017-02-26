using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummoningModule : MonoBehaviour {

    protected SummoningParameters p = null;

    protected List<GameObject> summons;
    protected float spawnCooldownTimer = 0.0f;
    // Ready to spawn based on cooldown timer
    protected bool spawnTimeReady;

    public void Update()
    {
        // Null guard
        if (p == null)
            return;

        if(!spawnTimeReady)
        {
            spawnCooldownTimer += Time.deltaTime;
            if(spawnCooldownTimer > p.spawnCooldown)
            {
                spawnTimeReady = true;
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

        GameObject summonedMonster = (GameObject)Instantiate(p.summonedGameObject, spawnPosition, Quaternion.identity);

        GenericMonsterBehaviour monsterBehaviour = summonedMonster.GetComponent<GenericMonsterBehaviour>();
        monsterBehaviour.pathFindingParameters.tileMap = p.tileMap;
        monsterBehaviour.pathFindingParameters.target = p.target;

        summons.Add(summonedMonster);

        spawnTimeReady = false;
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
        if (summons.Count < p.maxNumberOfSpawns)
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
        [Range(0.0f, 60.0f)]
        public float spawnCooldown;
        public TileMap tileMap;
        public GameObject target;
    }
}
