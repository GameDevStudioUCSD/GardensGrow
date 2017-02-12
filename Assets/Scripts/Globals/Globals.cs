using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Globals: MonoBehaviour {

    public const float pixelSize = 0.03125f;
    public enum Direction { North=0, South=1, East=2, West=3 };
    public enum Faction { Ally=0, Enemy=1}
    public static bool[] unlockedSeeds = {false, false, false, false, false, false, false, false, true};

    public struct PlantData : IComparable <PlantData>{
    	public Vector3 PlantLocation;
    	public string PlantScene;
        public Globals.Direction PlantDirection;

    	public PlantData(Vector3 location, string scene, Globals.Direction direction) {
    		PlantLocation = location;
    		PlantScene = scene;
            PlantDirection = direction;

    	}

    	public int CompareTo(PlantData other) {
    		if (this.PlantLocation.x < other.PlantLocation.x) {
    			return -1;
    		}
			else if (this.PlantLocation.x > other.PlantLocation.x) {
    			return 1;
    		}
			else if (this.PlantLocation.y < other.PlantLocation.y) {
    			return -1;
    		}
			else if (this.PlantLocation.y > other.PlantLocation.y) {
    			return 1;
    		}
			else if (this.PlantLocation.z < other.PlantLocation.z) {
    			return -1;
    		}
			else if (this.PlantLocation.z > other.PlantLocation.z) {
    			return 1;
    		}
    		else if (String.Compare(this.PlantScene, other.PlantScene) < 0) {
    			return -1;
    		}
			else if (String.Compare(this.PlantScene, other.PlantScene) > 0) {
    			return -1;
    		}
    		else {
    			return 0;
    		}
    	}
    }

    public static SortedList<PlantData, int> plants = new SortedList<PlantData, int>();

    public static PlayerGridObject player;

    public static string ground_tag = "Ground";
    public static string player_tag = "Player";

    public static string ground_layer = "Ground";

    public static int[] inventory = {0, 0, 0, 0, 0, 0, 0, 0};

    public static Vector3 spawnLocation = new Vector3(0.0f, -2.0f, 0.0f);

    public static Vector2 DirectionToVector(Direction direction)
    {
        Vector2 dirr = Vector2.up;
        switch (direction)
        {
            case Globals.Direction.East:
                dirr = Vector2.right;
                break;
            case Globals.Direction.West:
                dirr = Vector2.left;
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
    public static void SaveTheGame()
    {
        //KEEP PLANTS IN THE SAME ORDER FOR INSTANTIATION IN PLAYERGRIDOBJECT?
        //check which save 
        PlayerPrefsX.SetVector3("respawn", spawnLocation);
        PlayerPrefs.SetString("activeScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("playerHealth", player.health);

        PlayerPrefsX.SetIntArray("playerInventory", inventory);

        Vector3[] tempPlantPositions = new Vector3[plants.Count];
        int[] tempPlantTypes = new int[plants.Count];       //need plant type
        String[] tempPlantScenes = new String[plants.Count];
        int[] tempPlantDirections = new int[plants.Count];  //need plant object or direction

        int i = 0;
        foreach (KeyValuePair<PlantData, int> plantInfo in plants)
        {
            tempPlantPositions[i] = plantInfo.Key.PlantLocation;
            tempPlantScenes[i] = plantInfo.Key.PlantScene;
            tempPlantTypes[i] = plantInfo.Value;
            tempPlantDirections[i] = (int)plantInfo.Key.PlantDirection;
            i++;
        }

        PlayerPrefsX.SetVector3Array("PlantPositions", tempPlantPositions);
        PlayerPrefsX.SetIntArray("PlantTypes", tempPlantTypes);
        PlayerPrefsX.SetStringArray("PlantScenes", tempPlantScenes);
        PlayerPrefsX.SetIntArray("PlantDirections", tempPlantDirections);


    }
    public static void LoadTheGame()
    {
        //FIRST CLEAN THE SLATE AKA THE GAME STATE


        //check which save
        player.transform.position = PlayerPrefsX.GetVector3("respawn", spawnLocation);
        SceneManager.LoadScene(PlayerPrefs.GetString("activeScene", SceneManager.GetActiveScene().name));
        player.health = PlayerPrefs.GetInt("playerHealth", player.health);

        inventory = PlayerPrefsX.GetIntArray("playerInventory");

        Vector3[] tempPlantPositions = PlayerPrefsX.GetVector3Array("PlantPositions");
        int[] tempPlantTypes = PlayerPrefsX.GetIntArray("PlantTypes");
        String[] tempPlantScenes = PlayerPrefsX.GetStringArray("PlantScenes");
        int[] tempPlantDirections = PlayerPrefsX.GetIntArray("PlantDirections");

        for (int i = 0; i < tempPlantDirections.Length; i++)
        {
            plants.Add(new PlantData(tempPlantPositions[i], tempPlantScenes[i], (Direction)tempPlantDirections[i]), tempPlantTypes[i]);
            PlantGridObject newPlant = (PlantGridObject)Instantiate(player.plants[tempPlantTypes[i]],tempPlantPositions[i], Quaternion.identity);

   
        }
        
    }
}
