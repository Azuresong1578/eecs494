using UnityEngine;
using System.Collections;

public enum Direction
{
    down,
    left,
    up,
    right
}

public class Player : MonoBehaviour {

	public static Player S;
    public float moveSpeed;
    public int tileSize;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    public SpriteRenderer sprend;


    public bool ____________________;

    public RaycastHit hitInfo;

    public bool moving = false;
    public Vector3 targetPos;
    public Direction direction;
    public Vector3 moveVec;
    public bool onGrass = false;

    public Pokemon[] pokemonInBall = new Pokemon[6];


	void Awake()
    {
		S = this;
	}

    void Start()
    {
        sprend = gameObject.GetComponent<SpriteRenderer>();
    }

    public Rigidbody rigidBody
    {
        get { return gameObject.GetComponent<Rigidbody>(); }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    void FixedUpdate()
    {
        if (!moving && !Main.S.inDialog && !Main.S.paused)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                CheckForAction();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveVec = Vector3.right;
                direction = Direction.right;
                sprend.sprite = rightSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveVec = Vector3.left;
                direction = Direction.left;
                sprend.sprite = leftSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVec = Vector3.up;
                direction = Direction.up;
                sprend.sprite = upSprite;
                moving = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVec = Vector3.down;
                direction = Direction.down;
                sprend.sprite = downSprite;
                moving = true;
            }
            else
            {
                moveVec = Vector3.zero;
                moving = false;
            }

            if (Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] { "Immovable", "NPC" })))
            {
                moveVec = Vector3.zero;
                moving = false;
            }
            
            if (Physics.Raycast(GetRay(), out hitInfo, 0.5f, GetLayerMask(new string[] { "Grass" })))
            {
                onGrass = true;
            }
            targetPos = pos + moveVec;
        }
        else
        {
            if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime)
            {
                pos = targetPos;
                moving = false;
            }
            else
            {
                pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
            }
            if (onGrass && Random.value < 0.01 && !Main.S.paused)
            {
                Main.S.inBattle = true;
                Main.S.paused = true;
                Application.LoadLevelAdditive("_Scene_Battle");
            }
        }
    }

    Ray GetRay()
    {
        switch (direction)
        {
            case Direction.down:
                return new Ray(pos, Vector3.down);
            case Direction.left:
                return new Ray(pos, Vector3.left);
            case Direction.up:
                return new Ray(pos, Vector3.up);
            case Direction.right:
                return new Ray(pos, Vector3.right);
            default:
                return new Ray();
        }
    }

    int GetLayerMask(string[] layerNames) {
        int layerMask = 0;
        foreach(string layer in layerNames) {
            layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
        }
        return layerMask;
    }

    public void CheckForAction()
    {
        if (Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] { "NPC" })))
        {
            NPC npc = hitInfo.collider.gameObject.GetComponent<NPC>();
            npc.FacePlayer(direction);
            npc.PlayDialog();
        }
    }

    public void MoveThroughDoor(Vector3 doorLoc)
    {
        if (doorLoc.z <= 0) doorLoc.z = transform.position.z;
        moving = false;
        moveVec = Vector3.zero;
        transform.position = doorLoc;
    }

    public void swapPokemon()
    {

    }

	void Update(){

	}
}
