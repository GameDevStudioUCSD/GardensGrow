using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class SpeedModifier
{
    private Vector2 speed;

    public SpeedModifier(Vector2 speedVector)
    {
        speed = speedVector;
    }

    public void SetSpeed(float x, float y)
    {
        speed.x = x;
        speed.y = y;
    }

    public void SetSpeed(Vector2 speedVector)
    {
        speed = speedVector;
    }


    public Vector2 GetSpeed()
    {
        return speed;
    }
}
