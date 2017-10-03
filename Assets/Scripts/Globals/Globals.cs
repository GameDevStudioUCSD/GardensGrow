using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Globals: MonoBehaviour {

    //should be false in final build
    public static bool restartSaveState = false;

    public const float pixelSize = 0.03125f;
    public enum Direction { North=0, South=1, East=2, West=3 };
    public enum Faction { Ally=0, Enemy=1};
    public static int npcNum = 0;

    public static bool startCredits = false;

    public static bool lavaBossBeaten = false;
    public static bool windBossBeaten = false;
    public static bool caveBossBeaten = false;

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

    // Stuff That needs to be saved
    public static SortedList<PlantData, int> plants = new SortedList<PlantData, int>();

    public static int loadedSlot = 1;
    public static bool[] unlockedSeeds = {false, false, false, false, false, false, false, false, true};
	public static int[] inventory = {0, 0, 0, 0, 0, 0, 0, 0};
	public static int numKeys = 0;
	public static Vector3 spawnLocation = new Vector3(0.0f, -2.0f, 0.0f);

    public static PlayerGridObject player;
    public static UIController canvas;
    public static TileMap tileMap;

    public static string tile_map_tag = "TileMap";
    public static string ground_tag = "Ground";
    public static string player_tag = "Player";
    public static string room_tag = "Room";
    public static string ground_layer = "Ground";

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
    public static void SaveTheGame(int saveSlot) //should be 1-4
    {
        //KEEP PLANTS IN THE SAME ORDER FOR INSTANTIATION IN PLAYERGRIDOBJECT?
        PlayerPrefsX.SetVector3("respawn"+saveSlot, spawnLocation);
        PlayerPrefs.SetString("activeScene"+saveSlot, SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("playerHealth"+saveSlot, player.health);

        PlayerPrefsX.SetBool("lavaBossBeaten" + saveSlot, lavaBossBeaten);
        PlayerPrefsX.SetBool("windBossBeaten" + saveSlot, windBossBeaten);
        PlayerPrefsX.SetBool("caveBossBeaten" + saveSlot, caveBossBeaten);

        //Debug.Log("SAVING HEALTH AS " + player.health);
        PlayerPrefsX.SetIntArray("playerInventory"+saveSlot, inventory);

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
        //also save NPC states
        PlayerPrefsX.SetVector3Array("PlantPositions"+saveSlot, tempPlantPositions);
        PlayerPrefsX.SetIntArray("PlantTypes"+saveSlot, tempPlantTypes);
        PlayerPrefsX.SetStringArray("PlantScenes"+saveSlot, tempPlantScenes);
        PlayerPrefsX.SetIntArray("PlantDirections"+saveSlot, tempPlantDirections);

        DialogueNPCTrigger[] npcList = FindObjectsOfType<DialogueNPCTrigger>();
        

        foreach(DialogueNPCTrigger npc in npcList)
        {
            npc.saveBool(saveSlot);
        }

        DialogueTrigger[] signList = FindObjectsOfType<DialogueTrigger>();

        foreach (DialogueTrigger sign in signList)
        {
            sign.saveBool(saveSlot);
        }


    }
    public static int LoadTheGame(int loadSlot) //should be 1-4
    {
        //check if load exists, if not doesn't load the game
        if(PlayerPrefsX.GetVector3("respawn" + loadSlot) == null)
        {
            return -1; //failure
        }
        plants.Clear();
        loadedSlot = loadSlot;
        //LOADS THE GAME
        SceneManager.LoadScene(PlayerPrefs.GetString("activeScene" + loadSlot));

        //loadedSlot = loadSlot; TODO which spot?

        //change update player next respawn
        spawnLocation = PlayerPrefsX.GetVector3("respawn"+loadSlot);
        Globals.player.health = PlayerPrefs.GetInt("playerHealth"+loadSlot);
        inventory = PlayerPrefsX.GetIntArray("playerInventory"+loadSlot);

        PlayerPrefsX.GetBool("lavaBossBeaten" + loadSlot, lavaBossBeaten);
        PlayerPrefsX.GetBool("windBossBeaten" + loadSlot, windBossBeaten);
        PlayerPrefsX.GetBool("caveBossBeaten" + loadSlot, caveBossBeaten);

        Vector3[] tempPlantPositions = PlayerPrefsX.GetVector3Array("PlantPositions"+loadSlot);
        int[] tempPlantTypes = PlayerPrefsX.GetIntArray("PlantTypes"+loadSlot);
        String[] tempPlantScenes = PlayerPrefsX.GetStringArray("PlantScenes"+loadSlot);
        int[] tempPlantDirections = PlayerPrefsX.GetIntArray("PlantDirections"+loadSlot);


        for (int i = 0; i < tempPlantDirections.Length; i++)
        {
            plants.Add(new PlantData(tempPlantPositions[i], tempPlantScenes[i], (Direction)tempPlantDirections[i]), tempPlantTypes[i]);
        }

        return 1; //success
    }
}
