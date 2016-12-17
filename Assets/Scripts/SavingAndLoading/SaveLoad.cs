using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();
    public static int counter = 0;

    public static void Save()
    {
        if (counter<=4) //or </ number of max saves
        {
            savedGames.Add(Game.Current);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(counter + "/savedGames.gd");
            bf.Serialize(file, SaveLoad.savedGames);
            counter++;
            file.Close();
        }
        else
        {
            //throw error message   
        }
    }

    public static void Load(int loadnumber)
    {
        if(File.Exists(loadnumber + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(loadnumber + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            //here need to parse file, load level, and set all read file info
            file.Close();
        }
        else
        {
            //use IEnumerator of other object, attached public to this one, to display error message
        }
    }
}