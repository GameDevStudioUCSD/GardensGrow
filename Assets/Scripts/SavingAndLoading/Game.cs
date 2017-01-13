using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{

    public static Game Current;
    public PlayerGridObject player;

    public Game()
    {
        player = new PlayerGridObject();
        Application.LoadLevel(1);
    }

}
