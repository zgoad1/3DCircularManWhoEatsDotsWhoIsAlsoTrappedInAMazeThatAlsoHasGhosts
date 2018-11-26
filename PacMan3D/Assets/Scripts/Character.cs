﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Character : MonoBehaviour
{

    protected Rigidbody rb;
    protected Tile t;
    protected Tile tile {

        get {
            return t;
        }
        set {
            t = value;
            tPos.x = tile.transform.position.x;
            tPos.y = tile.transform.position.z;
        }
    }
    protected Tile nt;
    protected Tile nextTile
    {
        get
        {
            return nt;
        }
        set
        {
            nt = value;
            ntPos.x = nextTile.transform.position.x;
            ntPos.y = nextTile.transform.position.z;
        }
    }
    [Range(1, 4)]
    public int playerNum = 1;
    protected string hAxis, vAxis;
    [SerializeField]
    [Range(0, 31)]
    protected float speed = 24;
    public bool isAiOnly = false;
    [SerializeField]
    protected MapGene map;
    protected direction lasth = direction.RIGHT, lastv = direction.UP;            // last horizontal and vertical directions pressed
    protected direction lastdPress = direction.UP;
    protected Vector2 tPos = Vector2.zero, ntPos = Vector2.zero, myPos = Vector2.zero;
    protected static Vector3 left = new Vector3(-1, 0, 0), right = new Vector3(1, 0, 0), up = new Vector3(0, 0, 1), down = new Vector3(0, 0, -1);

    public enum direction
    {
        LEFT, RIGHT, UP, DOWN
    };

    protected void Reset()
    {
        map = FindObjectOfType<MapGene>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = false;
        rb.isKinematic = true;
        GetComponent<SphereCollider>().radius = 0.4f;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        Reset();
        Vector3 normCoords = GetNormalizedCoords(rb.position);
        tile = map.tileMap[MapGene.mapHeight - 1 - Mathf.FloorToInt(normCoords.z), Mathf.FloorToInt(normCoords.x)];
        nextTile = tile.up;
        hAxis = "P" + playerNum + "Horizontal";
        vAxis = "P" + playerNum + "Vertical";
    }

    // Update is called once per frame
    void Update()
    {
        myPos.x = rb.position.x;
        myPos.y = rb.position.z;
        // Stuff is reversed because I accidentally drew the map in the reverse order. Oops.
        if (!isAiOnly)
        {
            if (Input.GetAxisRaw(hAxis) > 0)
            {
                lasth = direction.LEFT;
                lastdPress = lasth;
                //Debug.Log("RIGHT pressed");
            }
            else if (Input.GetAxisRaw(hAxis) < 0)
            {
                lasth = direction.RIGHT;
                lastdPress = lasth;
                //Debug.Log("LEFT pressed");
            }
            if (Input.GetAxisRaw(vAxis) < 0)
            {
                lastv = direction.UP;
                lastdPress = lastv;
                //Debug.Log("DOWN pressed");
            }
            else if (Input.GetAxisRaw(vAxis) > 0)
            {
                lastv = direction.DOWN;
                lastdPress = lastv;
                //Debug.Log("UP pressed");
            }
        }
    }

    protected void FixedUpdate()
    {
        Move();
    }

    /*
	protected void OnDrawGizmos() {
		if(tile != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(tile.transform.position, 1);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(nextTile.transform.position, 1);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(rb.position, lastv == direction.UP ? rb.position + up * 2 : rb.position + down * 2);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(rb.position, lasth == direction.LEFT ? rb.position + left * 2 : rb.position + right * 2);
		}
	}
	*/

    protected Vector3 GetNormalizedCoords(Vector3 pos)
    {
        Vector3 normCoords = new Vector3(pos.x + MapGene.mapWidth * map.tileSize / 2.0f, pos.y, pos.z + MapGene.mapHeight * map.tileSize / 2.0f);
        return normCoords;
    }

    // Moving a character uses the method Rigidbody.MovePosition
    protected void Move()
    {
        float spd = 32 - speed;

        ////////////////////////////////
        #region Loop around screen check
        ////////////////////////////////
        float distFromTile = Vector2.Distance(myPos, tPos);
        if (tile.i == 0 && lastdPress == direction.UP || tile.i == MapGene.mapHeight - 1 && lastdPress == direction.DOWN)
        {
            // If we've passed the edge of the map, loop around to the other side
            if (distFromTile >= map.tileSize && distFromTile < 2 * map.tileSize)
            {
                rb.MovePosition(nextTile.transform.position - (lastdPress == direction.UP ? up : down) * map.tileSize);
                //new Vector3(rb.position.x, rb.position.y, -rb.position.z);	// this way caused bugs
            }
            else
            {
                // Not ready to loop around yet, keep going until we pass up this tile
                // (can't use nextTile in computations here since this is an edge case where nextTile is all the way across the map)
                if (lastdPress == direction.UP)
                {
                    rb.MovePosition(rb.position + up * map.tileSize / spd);
                }
                else
                {
                    rb.MovePosition(rb.position + down * map.tileSize / spd);
                }
            }
        }
        else if (tile.j == 0 && lastdPress == direction.LEFT || tile.j == MapGene.mapWidth - 1 && lastdPress == direction.RIGHT)
        {
            if (distFromTile >= map.tileSize && distFromTile < 2 * map.tileSize)
            {
                rb.MovePosition(nextTile.transform.position - (lastdPress == direction.LEFT ? left : right) * map.tileSize);
            }
            else
            {
                if (lastdPress == direction.LEFT)
                {
                    rb.MovePosition(rb.position + left * map.tileSize / spd);
                }
                else
                {
                    rb.MovePosition(rb.position + right * map.tileSize / spd);
                }
            }
            ////////////////////////////////
            #endregion
            ////////////////////////////////

        }
        else
        {
            // This fixes a bug with looping around the map
            if (tile.i == 0 && lastdPress == direction.DOWN || tile.i == MapGene.mapHeight - 1 && lastdPress == direction.UP ||
                    tile.j == 0 && lastdPress == direction.RIGHT || tile.j == MapGene.mapWidth - 1 && lastdPress == direction.LEFT)
            {
                if (distFromTile >= map.tileSize)
                {   // if we just looped around and then pressed the opposite direction
                    Tile temp = nextTile;
                    nextTile = tile;
                    tile = temp;
                    Debug.LogWarning("Attempting to fix bug #2");
                }
                else
                {                           // if we pressed the opposite direction right before looping around
                    SetNextTile();                  // Ask Zac for an explanation if you're really curious exactly what this check is for. If you're lucky, he'll remember.
                }
            }
            // Move in the direction from this tile to nextTile
            Vector3 movVec = (nextTile.transform.position - tile.transform.position) / spd;
            rb.MovePosition(rb.position + movVec);
            if (this is Enemy)
            {
                //Debug.Log("tile: " + tile + "nextTile: " + nextTile);
                //Debug.Log("moving " + movVec);
            }
        }
        if (Vector2.Distance(myPos, ntPos) <= map.tileSize / spd)
        {  // Reached a new tile
            ReachTile();
        }
    }

    protected virtual void ReachTile()
    {
        //Debug.Log("Initial tile: " + tile.gameObject.name);
        tile = nextTile;
        //Debug.Log("Moving to: " + tile.gameObject.name);
        rb.MovePosition(nextTile.transform.position);
        SetNextTile();
    }

    /**Get which direction we should go based on input and past input.
	 */
    protected virtual direction SetNextTile()
    {
        if (!isAiOnly)
        {
            if (lastdPress == direction.LEFT && tile.left.passable)
            {
                nextTile = tile.left;
                return direction.LEFT;
            }
            else if (lastdPress == direction.RIGHT && tile.right.passable)
            {
                nextTile = tile.right;
                return direction.RIGHT;
            }
            else if (lastdPress == direction.UP && tile.up.passable)
            {
                nextTile = tile.up;
                return direction.UP;
            }
            else if (lastdPress == direction.DOWN && tile.down.passable)
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
        }
        // Last input would hit a wall
        lastdPress = GetDefaultDirection(lastdPress);
      //  Debug.Log(name + " moving " + lastdPress);
        return lastdPress;
    }

    /**Returns the direction the player should go assuming the direction
	 * they want to go (d) is blocked.
	 */
    protected virtual direction GetDefaultDirection(direction d)
    {
        if (d == direction.LEFT)
        {
            if (lastv == direction.UP && tile.up.passable)
            {
                nextTile = tile.up;
                return direction.UP;
            }
            else if (lastv == direction.DOWN && tile.down.passable)
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
            else if (tile.up.passable)
            {
                nextTile = tile.up;
                return direction.UP;
            }
            else if (tile.down.passable)
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
            else
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.right;
                return direction.RIGHT;
            }
        }
        if (d == direction.RIGHT)
        {
            if (lastv == direction.UP && tile.up.passable)
            {
                nextTile = tile.up;
                return direction.UP;
            }
            else if (lastv == direction.DOWN && tile.down.passable)
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
            else if (tile.up.passable)
            {
                nextTile = tile.up;
                return direction.UP;
            }
            else if (tile.down.passable)
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
            else
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.left;
                return direction.LEFT;
            }
        }
        if (d == direction.UP)
        {
            if (lasth == direction.LEFT && tile.left.passable)
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.left;
                return direction.LEFT;
            }
            else if (lasth == direction.RIGHT && tile.right.passable)
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.right;
                return direction.RIGHT;
            }
            else if (tile.left.passable)
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.left;
                return direction.LEFT;
            }
            else if (tile.right.passable)
            {
                //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
                nextTile = tile.right;
                return direction.RIGHT;
            }
            else
            {
                nextTile = tile.down;
                return direction.DOWN;
            }
        }
        //if(d == direction.DOWN) {
        if (lasth == direction.LEFT && tile.left.passable)
        {
            //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
            nextTile = tile.left;
            return direction.LEFT;
        }
        else if (lasth == direction.RIGHT && tile.right.passable)
        {
            //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
            nextTile = tile.right;
            return direction.RIGHT;
        }
        else if (tile.left.passable)
        {
            //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
            nextTile = tile.left;
            return direction.LEFT;
        }
        else if (tile.right.passable)
        {
            //Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
            nextTile = tile.right;
            return direction.RIGHT;
        }
        else
        {
            nextTile = tile.up;
            return direction.UP;
        }
        //}
    }
}