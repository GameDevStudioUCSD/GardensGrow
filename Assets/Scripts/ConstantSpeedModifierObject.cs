using UnityEngine;
using System.Collections;

public class ConstantSpeedModifierObject : UncollidableStaticRotateableGridObject
{
    public float speed;
    public bool enabled;
    private SpeedModifier speedModifier;

    public override void Start()
    {
        speedModifier = new SpeedModifier(Globals.DirectionToVector(direction, speed));
        base.Start();
    }

    public override void ApplyEffect(MoveableGridObject obj)
    {
        if (enabled)
        {
            obj.AddSpeedModifier(speedModifier);
        }
    }

    public override void ClearEffect(MoveableGridObject obj)
    {
        if (enabled)
        {
            obj.RemoveSpeedModifier(speedModifier);
        }
    }
}
