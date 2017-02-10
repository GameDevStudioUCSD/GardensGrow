using UnityEngine;
using System.Collections;

public class Beaver : MonoBehaviour
{
    public Globals.Direction direction;
    private Quaternion rot;
    void Update()
    {
        if (direction == Globals.Direction.East)
        {
            rot = Quaternion.Euler(0, 0f, 0);
            this.gameObject.transform.rotation = rot;
        }
        else if (direction == Globals.Direction.West)
        {
            rot = Quaternion.Euler(0, 180f, 0);
            this.gameObject.transform.rotation = rot;
        }
    }

}
