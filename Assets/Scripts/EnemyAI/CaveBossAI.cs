using UnityEngine;
using System.Collections;

public class CaveBossAI : MonoBehaviour {

    public int BossHp = 10;
    private float moveWait = 3.0f; //boss waits x seconds before moving on to next location
    private float attackSpan = 2.0f; //boss lights up ALL circuits for x seconds

    public CircuitSystem[] circuitSystems;
    private float attackCD = 8.0f;   //boss waits x seconds before attacking again

    public bool canAttack = true;
    public bool stunned = false;

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
    void Start()
    {
        StartCoroutine(MovePattern());
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponentInParent<CircuitSystem>())
        {
            CircuitSystem cs = other.gameObject.GetComponentInParent<CircuitSystem>();
            if (canAttack)
            {
                //play attack animation
                if (cs.isLit)
                {
                    cs.isLit = false;
                    LightPlantObject[] lightList = FindObjectsOfType<LightPlantObject>();
                    foreach(LightPlantObject lp in lightList)
                    {
                        if (!lp.gameObject.GetComponent<LightSource>().belongsToRoom)
                        {
                            Destroy(lp.gameObject);
                        }
                    }
                }
                else
                {
                    StartCoroutine(Attack());
                }
                StartCoroutine(AttackCD());
            }
            else
            {
                if (cs.isLit)
                {
                    stunned = true;
                }
            }
        }

        if (other.gameObject.GetComponent<Boomerang>() && stunned)
        {
            //play damaged animation
            BossHp--;

            if (BossHp == 0)
            {
                //play death animation
                Destroy(this.gameObject);
            }   
        }
        
    }
    IEnumerator Attack()
    {
        //play attack animation
        foreach (CircuitSystem cs in circuitSystems)
        {
            cs.isLit = true;
            cs.ConnectJunction();
        }
        yield return new WaitForSeconds(attackSpan);

        foreach (CircuitSystem cs in circuitSystems)
        {
            cs.isLit = false;
            cs.DisconnectJunction();   
        }

        LightPlantObject[] lightList = FindObjectsOfType<LightPlantObject>();
        foreach (LightPlantObject lp in lightList)
        {
            if (!lp.gameObject.GetComponent<LightSource>().belongsToRoom)
            {
                Destroy(lp.gameObject);
            }
        }
    }
    IEnumerator AttackCD()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        stunned = false;
    }
    IEnumerator MovePattern()
    {
        while (true)
        {
            int i;
            /*Moves from top to right*/
            for (i = 0; i < 200; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.South);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 350; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.East);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 100; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.South);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(moveWait);
            /*Moves from right to bottom*/
            for (i = 0; i < 50; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.South);
                }
                else
                {
                   i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 350; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.West);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 250; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.South);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(moveWait);
            /*Moves from bottom to left*/
            for (i = 0; i < 175; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.West);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 100; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.North);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 125; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.West);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 200; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.North);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(moveWait);
            /*Move left to top*/
            for (i = 0; i < 125; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.North);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 100; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.East);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 175; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.North);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }
            for (i = 0; i < 200; i++)
            {
                if (!stunned)
                {
                    Move(Globals.Direction.East);
                }
                else
                {
                    i--;
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(moveWait);
        }
    }
    void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South )
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

    //TODO: probably won't use
    IEnumerator MoveMiddleToBottom()
    {
        for (int i = 0; i < 300; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator MoveMiddleToTop()
    {
        for (int i = 0; i < 300; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator MoveTopToRight()
    {
        int i;
        for (i = 0; i < 200; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 350; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 100; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator MoveRightToBottom()
    {
        int i;
        for (i = 0; i < 50; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 350; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 250; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator MoveBottomToLeft()
    {
        int i;
        for (i = 0; i < 175; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 100; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 125; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 200; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator MoveLeftToTop()
    {
        int i;
        for (i = 0; i < 125; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 100; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 175; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (i = 0; i < 200; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
    }
}
