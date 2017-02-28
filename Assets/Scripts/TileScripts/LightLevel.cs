using UnityEngine;
using System.Collections;

public class LightLevel : MonoBehaviour {

    public int level = 0;

    public void Brighten(int amount) {
        level += amount;
    }

    public void Dim(int amount) {
        level -= amount;
    }
}
