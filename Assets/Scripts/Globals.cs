using UnityEngine;

public class Globals
{
    public const float pixelSize = 0.03125f;

    public enum Direction
    {
        North,
        East,
        South,
        West,
        None,
        Northeast,
        Northwest,
        Southeast,
        Southwest
    };

    public static Vector2 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Globals.Direction.East:
                return Vector2.right;
            case Globals.Direction.West:
                return Vector2.left;
            case Globals.Direction.South:
                return Vector2.down;
            case Globals.Direction.North:
                return Vector2.up;
            case Direction.Northeast:
                return new Vector2(1.0f, 1.0f);
            case Direction.Northwest:
                return new Vector2(-1.0f, 1.0f);
            case Direction.Southeast:
                return new Vector2(1.0f, -1.0f);
            case Direction.Southwest:
                return new Vector2(-1.0f, -1.0f);
            default:
                return Vector2.zero;
        }
    }

    public static Vector2 DirectionToVector(Direction direction, float magnitude)
    {
        switch (direction)
        {
            case Globals.Direction.East:
                return new Vector2(magnitude, 0);
            case Globals.Direction.West:
                return new Vector2(-magnitude, 0);
            case Globals.Direction.South:
                return new Vector2(0, -magnitude);
            case Globals.Direction.North:
                return new Vector2(0, magnitude);
            case Direction.Northeast:
                return new Vector2(magnitude, magnitude);
            case Direction.Northwest:
                return new Vector2(-magnitude, magnitude);
            case Direction.Southeast:
                return new Vector2(magnitude, -magnitude);
            case Direction.Southwest:
                return new Vector2(-magnitude, -magnitude);
            default:
                return Vector2.zero;
        }
    }

    public static Direction VectorsToDirection(Vector2 currentPosition, Vector2 targetPosition)
    {
        if (targetPosition.x > currentPosition.x)
        {
            return Direction.East;
        }
        else if (targetPosition.x < currentPosition.x)
        {
            return Direction.West;
        }
        else if (targetPosition.y > currentPosition.y)
        {
            return Direction.North;
        }
        else if (targetPosition.y < currentPosition.y)
        {
            return Direction.South;
        }
        else
        {
            return Direction.None;
        }
    }
}