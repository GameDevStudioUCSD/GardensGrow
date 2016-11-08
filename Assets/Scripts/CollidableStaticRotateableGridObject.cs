using UnityEngine;
using System.Collections;

public class CollidableStaticRotateableGridObject : CollidableStaticGridObject, Rotateable {

    public Globals.Direction direction;

    public Globals.Direction GetDirection()
    {
        return direction;
    }

    public virtual void SetDirection(Globals.Direction dir)
    {
        direction = dir;
    }

    public virtual void Rotate(Globals.Direction dir)
    {
        SetDirection(dir);
    }
}
