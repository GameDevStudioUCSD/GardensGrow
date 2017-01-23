using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootTable : MonoBehaviour
{

    public List<LootTableEntry> entries;

    // Weight is the probability the item will drop compared to other items.
    // The higher the weight compared to other items, the higher the chance to drop.
    private int totalWeight = 0;

    public void Start()
    {
        foreach(LootTableEntry entry in entries)
        {
            totalWeight += entry.odds;
        }
    }

    public GameObject GetItem()
    {
        // If the loot table is empty, drop nothing
        if(entries.Count == 0)
        {
            return null;
        }

        int randomNumber = Random.Range(1, totalWeight);
        int currentSample = 0;

        // Sample the entries to find the item to drop
        // Each item in the entry has a range of values based on it's weight.
        // Go through each item and see if our random number falls into this range.
        // Items that fall into the range are the items that are dropped.
        foreach(LootTableEntry entry in entries)
        {
            currentSample += entry.odds;

            if (randomNumber <= currentSample)
                return entry.item;
        }

        // If set up correctly, we should never end up here
        return null;
    }

    [System.Serializable]
    public class LootTableEntry
    {
        public GameObject item;
        [Tooltip("The weight of getting this item to drop compared to other items.")]
        [Range(1, 1000)]
        public int odds = 1;
    }
}
