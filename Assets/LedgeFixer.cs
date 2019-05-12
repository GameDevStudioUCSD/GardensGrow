using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeFixer : MonoBehaviour
{
    public PlatformGridObject[] ledges;
    PlayerGridObject p;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<PlayerGridObject>();
    }

    public void moveLedges()
    {
        foreach (PlatformGridObject o in ledges)
        {
            o.gameObject.transform.position = o.origPos;
        }
    }
}
