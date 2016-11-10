using UnityEngine;
public class Globals {

    public const float pixelSize = 0.03125f;

    public enum Direction { North, East, South, West };

    public static string ground_tag = "Ground";
    public static string player_tag = "Player";

    public static string ground_layer = "Ground";

    public static int[] inventory = {0, 10000};

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

    public static Direction VectorsToDirection(Vector2 currentPosition, Vector2 targetPosition)
    {
        if(targetPosition.x > currentPosition.x)
        {
            return Direction.East;
        }
        else if(targetPosition.x < currentPosition.x)
        {
            return Direction.West;
        }
        else if(targetPosition.y > currentPosition.y)
        {
            return Direction.North;
        }
        else
        {
            return Direction.South;
        }
    }
}
