using UnityEngine;
using System.Collections.Generic;

public class CutsceneTrigger : MonoBehaviour {

    public string textFileName;

    /*NOTE: Ever time you place a new cutscene make sure to change the saveNumber
     *      in the inspector to a number not yet used (check the top of globals.cs 
     *      for saveNumbers that's already been used)
     */
    public int saveNumber;
    private int loadedSlot = -1;

    public bool conditionBeforeFirstBoss;
    public bool conditionAfterFirstBoss;
    public bool conditionAfterSecondBoss;
    public bool conditionAfterThirdBoss;

    public bool playedAlready = false;
    private bool isPlaying = false;
    private bool isEnding = false;

    public Vector3 playerPosition;
    public GameObject npc;
    public Vector3 npcStartPosition;
    public List<Globals.Direction> npcDirections;

    private GameObject dialogue;
    private Animator anim;
    private GameObject playerSpawned;
    private GameObject npcSpawned;

    private int pixelCounter = 32;
    private int directionCounter = -1;
    private Globals.Direction currentDirection;

    protected void Start() {
        if (Globals.canvas) dialogue = Globals.canvas.dialogUI;
        loadedSlot = Globals.loadedSlot;
        if (loadedSlot != -1) playedAlready = PlayerPrefsX.GetBool("cutscene" + saveNumber + "save" + loadedSlot);
        if (playedAlready) Destroy(this.gameObject); //cutscenes only play once
    }

    protected void Update() {
        if (!dialogue)
            dialogue = Globals.canvas.dialogUI;

        //NPC walking up to player before dialogue
        if (isPlaying) {
            if (pixelCounter >= 32) {
                directionCounter++;

                if (directionCounter >= npcDirections.Count) {
                    isPlaying = false;
                    npcSpawned.GetComponent<Animator>().SetInteger("Direction", 0);
                    ShowDialog();
                    return;
                }

                else {
                    pixelCounter = 0;
                    currentDirection = npcDirections[directionCounter];
                    //npcSpawned.GetComponent<Animator>().SetInteger("Direction", (int)currentDirection);
                    switch (currentDirection) {
                        case Globals.Direction.South:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 1);
                            break;
                        case Globals.Direction.North:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 2);
                            break;
                        default:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 0);
                            break;
                    }
                }
            }

            npcSpawned.GetComponent<MoveableGridObject>().Move(currentDirection);
            pixelCounter++;
        }

        //NPC walking away from player after dialogue
        if (isEnding) {
            if (pixelCounter >= 32) {
                directionCounter--;

                if (directionCounter < 0) {
                    isEnding = false;
                    playedAlready = true;
                    saveBool(loadedSlot);
                    //commmented out destory player

                    //Destroy(playerSpawned);
                    Destroy(npcSpawned);
                    Globals.player.gameObject.SetActive(true);
                    Destroy(this.gameObject);
                    return;
                }

                else {
                    pixelCounter = 0;
                    if (npcDirections[directionCounter] == Globals.Direction.North) currentDirection = Globals.Direction.South;
                    else if (npcDirections[directionCounter] == Globals.Direction.South) currentDirection = Globals.Direction.North;
                    else if (npcDirections[directionCounter] == Globals.Direction.East) currentDirection = Globals.Direction.West;
                    else if (npcDirections[directionCounter] == Globals.Direction.West) currentDirection = Globals.Direction.East;
                    //npcSpawned.GetComponent<Animator>().SetInteger("Direction", (int)currentDirection);
                    switch (currentDirection) {
                        case Globals.Direction.South:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 1);
                            break;
                        case Globals.Direction.North:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 2);
                            break;
                        default:
                            npcSpawned.GetComponent<Animator>().SetInteger("Direction", 0);
                            break;
                    }
                }
            }

            npcSpawned.GetComponent<MoveableGridObject>().Move(currentDirection);
            pixelCounter++;
        }
    }

    //Pause time, disable player, create cutscene player and npc, start walk up
    public void OnTriggerEnter2D(Collider2D other) {
        if (!playedAlready && other.gameObject == Globals.player.gameObject && EvaluateCondition()) {
            Globals.canvas.paused = true;
            playerSpawned = (GameObject)Instantiate(Globals.player.gameObject, playerPosition, Quaternion.identity);
            //Destroy(playerSpawned.GetComponent<PlayerGridObject>().gameObject);
            npcSpawned = (GameObject)Instantiate(npc, npcStartPosition, Quaternion.identity);
            DialogueNPCTrigger npcTrigger = npcSpawned.GetComponent<DialogueNPCTrigger>();
            if (npcTrigger) Destroy(npcTrigger);
            npcSpawned.AddComponent<MoveableGridObject>();
            npcSpawned.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            Globals.player.gameObject.SetActive(false);
            isPlaying = true;
        }
    }

    //Start dialogue
    private void ShowDialog() {
        Globals.canvas.ShowDialog(this);
        dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
        dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
    }

    //End dialogue, start walk away
    public void FinishCutscene() {
        isEnding = true;
    }

    public void saveBool(int saveSlot) {
        PlayerPrefsX.SetBool("cutscene" + saveNumber + "save" + saveSlot, playedAlready);
    }

    //Determines if cutscene will play based on conditions
    private bool EvaluateCondition() {
        if (conditionAfterThirdBoss && Globals.caveBossBeaten)
            return true;
        else if (conditionAfterSecondBoss && Globals.windBossBeaten)
            return true;
        else if (conditionAfterFirstBoss && Globals.lavaBossBeaten)
            return true;
        else if (conditionBeforeFirstBoss && !Globals.lavaBossBeaten)
            return true;
        else if (!conditionBeforeFirstBoss && !conditionAfterFirstBoss && !conditionAfterSecondBoss && !conditionAfterThirdBoss)
            return true;
        else
            return false;
    }
}
