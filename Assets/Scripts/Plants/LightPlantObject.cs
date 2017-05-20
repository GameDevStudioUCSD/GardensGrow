using UnityEngine;
using System.Collections;

public class LightPlantObject : PlantGridObject {

    //final boss stuff
    public bool evil = false;

    void OnDestroy()
    {
        if (evil)
        {
            CircuitSystem cs = FindObjectOfType<CircuitSystem>();
            cs.isLit = false;
            cs.DisconnectJunction();
        }
    }
}
