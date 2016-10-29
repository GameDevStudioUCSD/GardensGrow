using UnityEngine;

public class ActionState {
    public Vector2 position;
    public Globals.Direction direction;

    public ActionState(Vector2 position, Globals.Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }

    public override string ToString()
    {
        //return base.ToString();
        string output = "Position: " + position + " Direction: " + direction;

        return output;
    }
}
