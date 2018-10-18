using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Character : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 movVec = Vector3.zero;
	private Tile tile;
	private Tile nextTile;
	private int playerNum;
	private string hAxis, vAxis;
	[SerializeField] private MapGene map;
	private direction lasth = direction.RIGHT, lastv = direction.UP, lastd = direction.UP;	// last horizontal, last vertical, last direction

	private static int players = 1;

	enum direction {
		LEFT, RIGHT, UP, DOWN
	};

	private void Reset() {
		map = FindObjectOfType<MapGene>();
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		rb.useGravity = false;
	}

	// Use this for initialization
	void Start () {
		Reset();
		Vector3 normCoords = new Vector3(transform.position.x + map.mapWidth / 2, transform.position.y, transform.position.z + map.mapHeight / 2);
		tile = map.tileMap[map.mapHeight - 1 - Mathf.FloorToInt(normCoords.z), Mathf.FloorToInt(normCoords.x)];
		nextTile = tile.up;
		playerNum = players;
		players++;
		hAxis = "P" + playerNum + "Horizontal";
		vAxis = "P" + playerNum + "Vertical";
	}
	
	// Update is called once per frame
	void Update () {
		SetNextTile();
		Move();
	}

	// Moving a character uses the method Rigidbody.MovePosition
	public void Move() {
		// add some fraction of the direction vector from the current tile to the next tile
		Vector3 movVec = (nextTile.transform.position - tile.transform.position) / 16.0f;
		rb.MovePosition(rb.position + movVec);
		float dist1 = (rb.position - tile.transform.position).magnitude;
		float dist2 = (rb.position - nextTile.transform.position).magnitude;
		if(dist2 <= dist1) {
			tile = nextTile;
			direction newDirec = SetNextTile();
			if(newDirec == direction.UP || newDirec == direction.DOWN) {
				lastv = newDirec;
			} else {
				lasth = newDirec;
			}
			lastd = newDirec;
		}
	}

	private direction SetNextTile() {
		if(Input.GetAxisRaw(hAxis) > 0) {	// left and right are reversed for reasons
			if(tile.left.passable) {
				nextTile = tile.left;
				return direction.LEFT;
			} else {
				return GetDefaultDirection(direction.LEFT);
			}
		} else if(Input.GetAxisRaw(hAxis) < 0) {
			if(tile.right.passable) {
				nextTile = tile.right;
				return direction.RIGHT;
			} else {
				return GetDefaultDirection(direction.RIGHT);
			}
		} else if(Input.GetAxisRaw(vAxis) < 0) {
			if(tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else {
				return GetDefaultDirection(direction.UP);
			}
		} else if(Input.GetAxisRaw(vAxis) > 0) {
			if(tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			} else {
				return GetDefaultDirection(direction.DOWN);
			}
		}
		// No input
		return GetDefaultDirection(lastd);
	}

	/**Returns the direction the player should go assuming the direction
	 * they want to go (d) is blocked.
	 */
	private direction GetDefaultDirection(direction d) {
		if(d == direction.LEFT) {
			if(lastv == direction.UP && tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else if(lastv == direction.DOWN && tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			} else {
				nextTile = tile.right;
				return direction.RIGHT;
			}
		}
		if(d == direction.RIGHT) {
			if(lastv == direction.UP && tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else if(lastv == direction.DOWN && tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			} else {
				nextTile = tile.left;
				return direction.LEFT;
			}
		}
		if(d == direction.UP) {
			if(lasth == direction.LEFT && tile.left.passable) {
				nextTile = tile.left;
				return direction.LEFT;
			} else if(lasth == direction.RIGHT && tile.right.passable) {
				nextTile = tile.right;
				return direction.RIGHT;
			} else {
				nextTile = tile.down;
				return direction.DOWN;
			}
		}
		//if(d == direction.DOWN) {
		if(lasth == direction.LEFT && tile.left.passable) {
			nextTile = tile.left;
			return direction.LEFT;
		} else if(lasth == direction.RIGHT && tile.right.passable) {
			nextTile = tile.right;
			return direction.RIGHT;
		} else {
			nextTile = tile.up;
			return direction.UP;
		}
		//}
	}
}
