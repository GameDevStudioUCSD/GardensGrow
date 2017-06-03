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

    public bool playedAlready = false;
    private bool isPlaying = false;

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

    // Use this for initialization
    protected void Start() {
        if (playedAlready) Destroy(this.gameObject);
        dialogue = Globals.canvas.dialogUI;
        loadedSlot = Globals.loadedSlot;
        if (loadedSlot != -1) playedAlready = PlayerPrefsX.GetBool("cutscene" + saveNumber + "save" + loadedSlot);
    }

    // Update is called once per frame
    protected void Update() {
        if (isPlaying) {
            if (pixelCounter >= 32) {
                pixelCounter = 0;
                directionCounter++;

                if (directionCounter >= npcDirections.Count) {
                    isPlaying = false;
                    ShowDialog();
                    return;
                }

                else {
                    npc.GetComponent<Animator>().SetInteger("Direction", (int)npcDirections[directionCounter]);
                    currentDirection = npcDirections[directionCounter];
                }
            }

            npcSpawned.GetComponent<MoveableGridObject>().Move(currentDirection);
            pixelCounter++;
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (!playedAlready && other.gameObject == Globals.player.gameObject) {
            Globals.canvas.paused = true;
            playerSpawned = (GameObject)Instantiate(Globals.player.gameObject, playerPosition, Quaternion.identity);
            Destroy(playerSpawned.GetComponent<PlayerGridObject>());
            npcSpawned = (GameObject)Instantiate(npc, npcStartPosition, Quaternion.identity);
            DialogueNPCTrigger npcTrigger = npcSpawned.GetComponent<DialogueNPCTrigger>();
            if (npcTrigger) Destroy(npcTrigger);
            npcSpawned.AddComponent<MoveableGridObject>();
            Globals.player.gameObject.SetActive(false);
            isPlaying = true;
        }
    }

    private void ShowDialog() {
        Globals.canvas.ShowDialog(this);
        dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
        dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
    }

    public void FinishCutscene() {
        playedAlready = true;
        saveBool(loadedSlot);
        Destroy(playerSpawned);
        Destroy(npcSpawned);
        Globals.player.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

    public void saveBool(int saveSlot) {
        PlayerPrefsX.SetBool("cutscene" + saveNumber + "save" + saveSlot, playedAlready);
    }
}
