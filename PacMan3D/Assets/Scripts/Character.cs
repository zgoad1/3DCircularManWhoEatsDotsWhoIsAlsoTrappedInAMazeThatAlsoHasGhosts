using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Character : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 movVec = Vector3.zero;
	private Tile t;
	private Tile tile {

		get {
			return t;
		}
		set {
			t = value;
			tPos.x = tile.transform.position.x;
			tPos.y = tile.transform.position.z;
		}
	}
	private Tile nt;
	private Tile nextTile {
		get {
			return nt;
		}
		set {
			nt = value;
			ntPos.x = nextTile.transform.position.x;
			ntPos.y = nextTile.transform.position.z;
		}
	}
	[Range(2, 4)] public int playerNum = 1;
	private string hAxis, vAxis;
	[SerializeField] [Range(0, 31)] private float speed = 24;
	public bool isAiOnly = false;
	public bool isPacman = false;
	[SerializeField] private MapGene map;
	private direction lasth = direction.RIGHT, lastv = direction.UP;			// last horizontal and vertical directions pressed
	private direction lastdPress = direction.UP, lastdTravel = direction.UP;    // last direction pressed and last direction traveled
	private direction lastDecided = direction.UP;
	private float percent = 0;  // how far we are from tile to nextTile
	private Vector2 tPos = Vector2.zero, ntPos = Vector2.zero, myPos = Vector2.zero;
	private static Vector3 left = new Vector3(-1, 0, 0), right = new Vector3(1, 0, 0), up = new Vector3(0, 0, 1), down = new Vector3(0, 0, -1);

	enum direction {
		LEFT, RIGHT, UP, DOWN
	};

	private void Reset() {
		map = FindObjectOfType<MapGene>();
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		rb.useGravity = false;
		rb.isKinematic = true;
		GetComponent<SphereCollider>().radius = 0.4f;
	}

	// Use this for initialization
	void Start () {
		Reset();
		Vector3 normCoords = GetNormalizedCoords(rb.position);
		tile = map.tileMap[map.mapHeight - 1 - Mathf.FloorToInt(normCoords.z), Mathf.FloorToInt(normCoords.x)];
		nextTile = tile.up;
		hAxis = "P" + playerNum + "Horizontal";
		vAxis = "P" + playerNum + "Vertical";
	}
	
	// Update is called once per frame
	void Update () {
		myPos.x = rb.position.x;
		myPos.y = rb.position.z;
		// Stuff is reversed because I accidentally drew the map in the reverse order. Oops.
		if(Input.GetAxisRaw(hAxis) > 0) {
			lasth = direction.LEFT;
			lastdPress = lasth;
			//Debug.Log("RIGHT pressed");
		} else if(Input.GetAxisRaw(hAxis) < 0) {
			lasth = direction.RIGHT;
			lastdPress = lasth;
			//Debug.Log("LEFT pressed");
		}
		if(Input.GetAxisRaw(vAxis) < 0) {
			lastv = direction.UP;
			lastdPress = lastv;
			//Debug.Log("DOWN pressed");
		} else if(Input.GetAxisRaw(vAxis) > 0) {
			lastv = direction.DOWN;
			lastdPress = lastv;
			//Debug.Log("UP pressed");
		}
	}

	private void FixedUpdate() {
		Move();
	}

	private void OnDrawGizmos() {
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

	private Vector3 GetNormalizedCoords(Vector3 pos) {
		Vector3 normCoords = new Vector3(pos.x + map.mapWidth * map.tileSize / 2.0f, pos.y, pos.z + map.mapHeight * map.tileSize / 2.0f);
		return normCoords;
	}

	// Moving a character uses the method Rigidbody.MovePosition
	private void Move() {
		float spd = 32 - speed;
		if(tile.i == 0 && lastdPress == direction.UP || tile.i == map.mapHeight - 1 && lastdPress == direction.DOWN) {
			float distFromTile = Vector2.Distance(myPos, tPos);
			// If we've passed the edge of the map, loop around to the other side
			if(distFromTile >= map.tileSize && distFromTile < 2 * map.tileSize) {
				rb.MovePosition(new Vector3(-rb.position.x, rb.position.y, rb.position.z));
			} else {
				// Not ready to loop around yet, keep going until we pass up this tile
				if(lastdPress == direction.UP) {
					rb.MovePosition(rb.position + up * map.tileSize / spd);
				} else {
					rb.MovePosition(rb.position + down * map.tileSize / spd);
				}
			}
		} else if(tile.j == 0 && lastdPress == direction.LEFT || tile.j == map.mapWidth - 1 && lastdPress == direction.RIGHT) {
			float distFromTile = Vector2.Distance(myPos, tPos);
			if(distFromTile >= map.tileSize && distFromTile < 2 * map.tileSize) {
				rb.MovePosition(new Vector3(-rb.position.x, rb.position.y, rb.position.z));
			} else {
				if(lastdPress == direction.LEFT) {
					rb.MovePosition(rb.position + left * map.tileSize / spd);
				} else {
					rb.MovePosition(rb.position + right * map.tileSize / spd);
				}
			}
		} else {
			if(tile.i == 0 && lastdPress == direction.DOWN || tile.i == map.mapHeight - 1 && lastdPress == direction.UP ||
					tile.j == 0 && lastdPress == direction.RIGHT || tile.j == map.mapWidth - 1 && lastdPress == direction.LEFT) {
				SetNextTile();	// Ask Zac for an explanation if you're really curious what this check is for. If you're lucky, he'll remember.
			}
			// Move in the direction from this tile to nextTile
			Vector3 movVec = (nextTile.transform.position - tile.transform.position) / spd;
			rb.MovePosition(rb.position + movVec);
		}
		if(Vector2.Distance(myPos, ntPos) <= map.tileSize / spd) {	// Reached a new tile
			//Debug.Log("Initial tile: " + tile.gameObject.name);
			tile = nextTile;
			//Debug.Log("Moving to: " + tile.gameObject.name);
			rb.MovePosition(nextTile.transform.position);
			direction newDirec = SetNextTile();
			lastdTravel = newDirec;
		}
	}

	/**Get which direction we should go based on input and past input.
	 */
	private direction SetNextTile() {
		if(!isAiOnly) {
			if(lastdPress == direction.LEFT && tile.left.passable) {
				nextTile = tile.left;
				return direction.LEFT;
			} else if(lastdPress == direction.RIGHT && tile.right.passable) {
				nextTile = tile.right;
				return direction.RIGHT;
			} else if(lastdPress == direction.UP && tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else if(lastdPress == direction.DOWN && tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			}
		}
		// Last input would hit a wall
		lastdPress = GetDefaultDirection(lastdPress);
		return lastdPress;
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
			} else if(tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else if(tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			} else {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
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
			} else if(tile.up.passable) {
				nextTile = tile.up;
				return direction.UP;
			} else if(tile.down.passable) {
				nextTile = tile.down;
				return direction.DOWN;
			} else {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
				nextTile = tile.left;
				return direction.LEFT;
			}
		}
		if(d == direction.UP) {
			if(lasth == direction.LEFT && tile.left.passable) {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
				nextTile = tile.left;
				return direction.LEFT;
			} else if(lasth == direction.RIGHT && tile.right.passable) {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
				nextTile = tile.right;
				return direction.RIGHT;
			} else if(tile.left.passable) {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
				nextTile = tile.left;
				return direction.LEFT;
			} else if(tile.right.passable) {
				//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
				nextTile = tile.right;
				return direction.RIGHT;
			} else { 
				nextTile = tile.down;
				return direction.DOWN;
			}
		}
		//if(d == direction.DOWN) {
		if(lasth == direction.LEFT && tile.left.passable) {
			//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
			nextTile = tile.left;
			return direction.LEFT;
		} else if(lasth == direction.RIGHT && tile.right.passable) {
			//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
			nextTile = tile.right;
			return direction.RIGHT;
		} else if(tile.left.passable) {
			//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
			nextTile = tile.left;
			return direction.LEFT;
		} else if(tile.right.passable) {
			//Debug.Log("lastv: " + lastv + ", lasth: " + lasth + ", lastdTravel: " + lastdTravel + ", lastdPress: " + lastdPress + "\nUp passable: " + tile.up.passable + ", Down passable: " + tile.down.passable);
			nextTile = tile.right;
			return direction.RIGHT;
		} else {
			nextTile = tile.up;
			return direction.UP;
		}
		//}
	}
}
