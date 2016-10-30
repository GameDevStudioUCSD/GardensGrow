using UnityEngine;
public class Globals {

    public const float pixelSize = 0.03125f;

    public enum Direction { North, East, South, West };
    public static Vector2 DirectionToVector(Direction direction)
    {
        Vector2 dirr = Vector2.up;
        switch (direction)
        {
            case Globals.Direction.East:
                dirr = Vector2.right;
                break;
            case Globals.Direction.West:
                dirr = -Vector2.right;
                break;
            case Globals.Direction.South:
                dirr = Vector2.down;
                break;
            case Globals.Direction.North:
                dirr = Vector2.up;
                break;
        }
        return dirr;
    }
}
