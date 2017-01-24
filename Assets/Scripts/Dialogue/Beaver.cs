using UnityEngine;
using System.Collections;

public class Beaver : MonoBehaviour
{
    public Globals.Direction direction;
   
    void Update()
    {
        if (direction == Globals.Direction.East)
        {
            this.gameObject.transform.Rotate(new Vector3(0, 0, 0));
        }
        else if (direction == Globals.Direction.West)
        {

            this.gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

}
