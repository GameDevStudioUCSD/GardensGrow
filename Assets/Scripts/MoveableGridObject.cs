using UnityEngine;
using System.Collections;

public class MoveableGridObject : KillableGridObject {

	public PlayerEdgeTrigger southCollider;
	public PlayerEdgeTrigger westCollider;
	public PlayerEdgeTrigger northCollider;
	public PlayerEdgeTrigger eastCollider;
	private const float pixelSize = Globals.pixelSize;

	private bool southCollision = false;
	private bool westCollision = false;
	private bool northCollision = false;
	private bool eastCollision = false;

    public Sprite IdleSpriteSouth;
    public Sprite IdleSpriteWest;
    public Sprite IdleSpriteNorth;
    public Sprite IdleSpriteEast;
    public int walkSpriteFrameCount = 10;
    protected int currentFrame = 0;
    protected int currentSprite = 0;
    public System.Collections.Generic.List<Sprite> WalkSpriteSouth;
    public System.Collections.Generic.List<Sprite> WalkSpriteWest;
    public System.Collections.Generic.List<Sprite> WalkSpriteNorth;
    public System.Collections.Generic.List<Sprite> WalkSpriteEast;

	protected virtual void Update() {
		base.Update();
	}

	// Direction: 0 = South, 1 = West, 2 = North, 3 = East
	public virtual void Move(Globals.Direction direction) {
		Rotate(direction);
		if (direction == Globals.Direction.South && !southCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.South) {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount) {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteSouth.Count) currentSprite = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteSouth[currentSprite];
                }
            }
            else {
                direction = Globals.Direction.South;
                currentFrame = 0;
                currentSprite = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteSouth[0];
            }
        }
		else if (direction == Globals.Direction.West && !westCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.West) {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount) {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteWest.Count) currentSprite = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteWest[currentSprite];
                }
            }
            else {
                direction = Globals.Direction.West;
                currentFrame = 0;
                currentSprite = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteWest[0];
            }
        }
		else if (direction == Globals.Direction.North && !northCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.North) {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount) {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteNorth.Count) currentSprite = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteNorth[currentSprite];
                }
            }
            else {
                direction = Globals.Direction.North;
                currentFrame = 0;
                currentSprite = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteNorth[0];
            }
        }
		else if (direction == Globals.Direction.East && !eastCollider.isTriggered) {
			Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.East)
            {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount)
                {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteEast.Count) currentSprite = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteEast[currentSprite];
                }
            }
            else
            {
                direction = Globals.Direction.East;
                currentFrame = 0;
                currentSprite = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = WalkSpriteEast[0];
            }
        }
	}
    public virtual void MoveEnemy(Globals.Direction direction)
    {
        Rotate(direction);
        if (direction == Globals.Direction.South && !southCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.y -= pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.South)
            {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount)
                {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteSouth.Count) currentSprite = 0;
                  
                }
            }
            else
            {
                direction = Globals.Direction.South;
                currentFrame = 0;
                currentSprite = 0;
                
            }
        }
        else if (direction == Globals.Direction.West && !westCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.x -= pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.West)
            {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount)
                {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteWest.Count) currentSprite = 0;
                    
                }
            }
            else
            {
                direction = Globals.Direction.West;
                currentFrame = 0;
                currentSprite = 0;
                
            }
        }
        else if (direction == Globals.Direction.North && !northCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.y += pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.North)
            {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount)
                {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteNorth.Count) currentSprite = 0;
                    
                }
            }
            else
            {
                direction = Globals.Direction.North;
                currentFrame = 0;
                currentSprite = 0;
                
            }
        }
        else if (direction == Globals.Direction.East && !eastCollider.isTriggered)
        {
            Vector3 position = this.transform.position;
            position.x += pixelSize;
            this.transform.position = position;
            if (direction == Globals.Direction.East)
            {
                currentFrame++;
                if (currentFrame >= walkSpriteFrameCount)
                {
                    currentFrame = 0;
                    currentSprite++;
                    if (currentSprite >= WalkSpriteEast.Count) currentSprite = 0;
                    
                }
            }
            else
            {
                direction = Globals.Direction.East;
                currentFrame = 0;
                currentSprite = 0;
                
            }
        }
    }
    protected virtual void Stop() {
        if (direction == Globals.Direction.South)
            gameObject.GetComponent<SpriteRenderer>().sprite = IdleSpriteSouth;
        else if (direction == Globals.Direction.West)
            gameObject.GetComponent<SpriteRenderer>().sprite = IdleSpriteWest;
        else if (direction == Globals.Direction.North)
            gameObject.GetComponent<SpriteRenderer>().sprite = IdleSpriteNorth;
        else if (direction == Globals.Direction.East)
            gameObject.GetComponent<SpriteRenderer>().sprite = IdleSpriteEast;
    }
}