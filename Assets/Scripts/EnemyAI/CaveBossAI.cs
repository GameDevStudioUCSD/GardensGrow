using UnityEngine;
using System.Collections;

public class CaveBossAI : MonoBehaviour {

    public int BossHp = 10;

    public CircuitSystem[] circuitSystems;

    public GameObject emblem;

    private float attackCD = 8.0f;   //boss waits x seconds before attacking again

    private bool canAttack = true;
    private bool stunned = false;
    private bool attacking = false;

    private float attackDuration = 2.0f; //boss lights up ALL circuits for x seconds
    private float stunDuration = 3.0f;

    private Animator anim;


    /*BOSS MOVE PATTERN:
     * Middle:0,0,0
     * Top Room: 0,9.375,0
     * Bottom Room: 0,-9.375,0
     * East Room: 10.937,0,0
     * West Room: -9.375,0,0
     */

     /*
      * Mechanics:
    Boss can attack every 8 seconds
        If reaches an off junction
            Turn on all circuits to try to hit player for x seconds, then turn off all
            circuits and turn off all lightplants that u planted

        Else if reaches an on junction
            Turn off all lightplants that u planted

    How to beat boss:
        Turn on a lightplant while the boss is standing a circuit while the boss’s
        attack is on cooldown, then the boss is stunned for x seconds, then in only this
        time frame can the boss be hittable by the boomerang
       */
    void Start() {
        StartCoroutine(MovePattern());
        anim = this.gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponentInParent<CircuitSystem>() != null) {
            if (canAttack) {
                anim.SetInteger("AnimNum", 0);
                StartCoroutine(Attack());
                StartCoroutine(AttackCD());
            }
        }

        if (other.gameObject.GetComponent<Boomerang>() != null) {
            if (stunned) {
                BossHp--;
                StartCoroutine(DamagedAnim());
                if (BossHp == 0) {
                    StartCoroutine(DeathAnim());
                }
            }
            else {
                stunned = true;
                StartCoroutine(StopStun());
            }
        }
    }

    IEnumerator DamagedAnim() {
        anim.SetInteger("AnimNum", 3);
        yield return new WaitForSeconds(1.0f);
        anim.SetInteger("AnimNum", 1);
    }

    IEnumerator Attack() {
        //play attack animation
        foreach (CircuitSystem cs in circuitSystems)
        {
            cs.isLit = true;
            cs.ConnectJunction();
        }
        attacking = true;

        yield return new WaitForSeconds(attackDuration);

        foreach (CircuitSystem cs in circuitSystems)
        {
            cs.isLit = false;
            cs.DisconnectJunction();
        }
        attacking = false;
    }

    IEnumerator StopStun() {
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
    }

    IEnumerator DeathAnim() {
    	emblem.SetActive(true);
        anim.SetInteger("AnimNum", 2);
        yield return new WaitForSeconds(4.0f);
        Destroy(this.gameObject);
    }

    IEnumerator AttackCD() {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        anim.SetInteger("AnimNum", 0);
    }

    IEnumerator MovePattern() {
        while (true) {
            int i;

            // Moves from top to right
            for (i = 0; i < 160; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.South);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 128; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.East);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.South);
                }
                yield return new WaitForEndOfFrame();
            }

            // Moves from right to bottom
            for (i = 0; i < 128; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.South);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 128; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.West);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 32; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.South);
                }
                yield return new WaitForEndOfFrame();
            }

            // Moves from bottom to left
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.West);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.North);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.West);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.North);
                }
                yield return new WaitForEndOfFrame();
            }

            // Move left to top
            for (i = 0; i < 288; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.North);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 128; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.East);
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 64; i++) {
                if (attacking || stunned) {
                    --i;
                }
                else {
                    Move(Globals.Direction.South);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize;
            this.transform.position = position;
        }
    }
}
