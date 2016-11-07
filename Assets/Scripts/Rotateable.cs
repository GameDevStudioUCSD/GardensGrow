using UnityEngine;
using System.Collections;

public interface Rotateable
{
    Globals.Direction GetDirection();
    void SetDirection(Globals.Direction direction);
    void Rotate(Globals.Direction direction);
}
