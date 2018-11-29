using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : Character {
	protected direction where = (direction)(-5);
	private Node start, goal;
	private static direction[] actions = new direction[] { direction.DOWN, direction.LEFT, direction.RIGHT, direction.UP };


	protected override void Start() {
		base.Start();
		TileData[,] startWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth], goalWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth];
		for(int i = 0; i < MapGene.mapHeight; i++) {
			for(int j = 0; j < MapGene.mapWidth; j++) {
				Item.ItemType t = map.tileMap[i, j].item == null ? Item.ItemType.NONE : map.tileMap[i, j].item.GetComponent<Item>().type;
				startWorld[i, j] = new TileData(t, i, j, map.tileMap[i, j].passable);
				goalWorld[i, j] = new TileData(Item.ItemType.NONE, i, j, map.tileMap[i, j].passable);
			}
		}
		Vector3 normCoords = GetNormalizedCoords(rb.position);
		Vector2Int coords = new Vector2Int((int)normCoords.z, (int)normCoords.x);
		start = new Node(startWorld, coords, where);
		goal = new Node(goalWorld, coords, where);
		start.goal = goal;
		ReachTile();
	}

	private void OnTriggerEnter(Collider other) {
		// make items disappear upon hitting them
		if(other.gameObject.GetComponent<Item>() != null) {
			other.GetComponent<Renderer>().enabled = false;
			if(other.gameObject.GetComponent<Item>().type == Item.ItemType.BIGDOT) {
				ChangeState(state.REVERSE);
			}
		} else if(other.gameObject.GetComponent<Character>() != null) {
			if(charState == state.NORMAL) {
				// game over for Pac-Man
				FindObjectOfType<ExplosionManager>().GoBoom();
			} else {
				// kill a ghost
				other.gameObject.GetComponent<Character>().Relocate();
			}
		}
	}

	protected override direction GetDefaultDirection(direction d) {
		switch(where) {
			case direction.DOWN:
				nextTile = tile.down;
				break;
			case direction.LEFT:
				nextTile = tile.left;
				break;
			case direction.RIGHT:
				nextTile = tile.right;
				break;
			default:
				nextTile = tile.up;
				break;
		}
		return where;
	}

	protected override void ReachTile() {
		//tile.item.GetComponent<Item>().type = Item.ItemType.NONE;
		Destroy(tile.item);
		tile.item = null;
		Vector3 normCoords = GetNormalizedCoords(rb.position);
		Vector2Int coords = new Vector2Int((int)normCoords.z, (int)normCoords.x);
		start.agentPos = coords;
		start.world[coords.x, coords.y] = goal.world[coords.x, coords.y];
		goal.agentPos = GetNearestGhostPos(coords);
		Astar(start);
		base.ReachTile();
	}

	// d = distance to "dirty"
	// w = # of "dirty" tiles
	protected float Heuristic(float d, int w, Vector2Int agentPos) {
		// this heuristic is perfectly accurate assuming all the
		// "dirty tiles" can be "succed" contiguously
		float dist = GetDistPenalty(agentPos);
		return d * (w * 2 + 1) + w * (w + 1) + (dist * (dist + 1)) / 2;
	}

	public static Vector2Int GetNearestGhostPos(Vector2Int agentPos) {
		float dist = Mathf.Infinity; // distance to nearest ghost
		Vector2Int nearest = ghostPositions[instance];
		foreach(Character g in ghostPositions.Keys) {
			float newDist = Vector2Int.Distance(agentPos, ghostPositions[g]);
			if(newDist < dist) {
				dist = newDist;
				nearest = ghostPositions[g];
			}
		}
		return nearest;
	}

	// get the penalty point cost for proximity to ghosts
	public static float GetDistPenalty(Vector2Int agentPos) {
		//if(charState == state.REVERSE) return 0;
		float dist = Vector2Int.Distance(agentPos, GetNearestGhostPos(agentPos));
		return Mathf.Max(0, charState == state.NORMAL ? 8 - dist : dist);
	}

	private Node GetMinFscore(List<Node> list) {
		Node minfscore = list[0];
		foreach(Node n in list) {
			if(n.fscore < minfscore.fscore) minfscore = n;
		}
		return minfscore;
	}

	private Node GetNextNode(Node c) {
		if(c.cameFrom == null) {
			FindObjectOfType<ExplosionManager>().GoBoom();
			return c;
		}
		if(c.cameFrom.cameFrom == null)
			return c;
		return GetNextNode(c.cameFrom);
	}

	private void Astar(Node start) {
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>();
		openSet.Add(start);
		start.gscore = 0;
		start.fscore = Heuristic(start.DistToItem(), start.GetItemTiles().Count, start.agentPos);
		int iterations = 0;
		while(openSet.Count > 0) {
			Node current = GetMinFscore(openSet);

			// stop us from looping forever in a very bad programmer way
			iterations++;

			if(current == goal || iterations >= 50) {
				Node next = GetNextNode(current);
				where = next.actionPerformed;
				//start = next;
				//start.cameFrom = null;  // forget the past. there is only the future.
										// (keeps GetNextNode from messing up)
				Debug.Log("Astar chose " + next.actionPerformed);
				return;
			}
			openSet.Remove(current);
			closedSet.Add(current);

			current.neighbors = new List<Node>();
			foreach(direction a in actions) {
				//if(!OppositeDirection(a, current.actionPerformed)) {
					Node newNeighbor = current.affect(a);
					if(newNeighbor != null)
						current.neighbors.Add(newNeighbor);
				//}
			}

			foreach(Node n in current.neighbors) {
				if(closedSet.Contains(n))
					continue;
				float tempgscore = current.gscore + current.GetActionCost(n.actionPerformed);
				if(!openSet.Contains(n)) {
					openSet.Add(n);
				} else if(tempgscore >= n.gscore) {
					continue;
				}

				n.gscore = tempgscore;
				n.fscore = n.gscore + Heuristic(n.DistToItem(), n.GetItemTiles().Count, n.agentPos);// + (OppositeDirection(n.actionPerformed, current.actionPerformed) ? 1 : 0);
			}
			//Debug.Log("caete");
		}
	}
}

class TileData {
	public Item.ItemType item;
	public Vector2Int ij;
	public bool passable;

	public TileData(Item.ItemType item, Vector2Int coords, bool passable) {
		this.item = item;
		ij = coords;
		this.passable = passable;
	}
	public TileData(Item.ItemType item, int i, int j, bool passable) {
		this.item = item;
		ij = new Vector2Int(i, j);
		this.passable = passable;
	}

	public static bool operator ==(TileData thys, TileData that) {
		return thys.item == that.item && thys.ij == that.ij;
	}
	public static bool operator !=(TileData thys, TileData that) {
		return !(thys.item == that.item && thys.ij == that.ij);
	}

	public TileData Copy() {
		return new TileData(item, ij, passable);
	}
}

class Node {
	public float fscore = Mathf.Infinity, gscore = Mathf.Infinity;
	public List<Node> neighbors;
	public TileData[,] world = new TileData[MapGene.mapHeight, MapGene.mapWidth];
	public Vector2Int agentPos;
	public Character.direction actionPerformed;
	public Node cameFrom;
	public Node goal;

	List<TileData> itemTiles;


	public Node(TileData[,] world, Vector2Int agentPos, Character.direction action) {
		this.world = world;
		this.agentPos = agentPos;
		actionPerformed = action;
	}

	public static bool operator ==(Node n1, Node n2) {
		if(n1 is Node ^ n2 is Node) {
			// one or the other is null, but not both
			return false;
		}
		if(!(n1 is Node)) {
			// both are null
			return true;
		}
		// both are nodes
		if(n1.GetItemTiles().Count == 0 && n2.GetItemTiles().Count == 0)
			return true;
		if(Character.charState == Character.state.NORMAL) {
			// use normal comparison
			//Debug.Log("n1: " + n1.GetItemTiles().Count + ", n2: " + n2.GetItemTiles().Count);
			for(int i = 0; i < MapGene.mapHeight; i++) {
				for(int j = 0; j < MapGene.mapWidth; j++) {
					if(n1.world[i, j].item != n2.world[i, j].item)
						return false;
				}
			}
		return n1.agentPos == n2.agentPos;
		} else {
			// node is goal if agentPos is the same
			return n1.agentPos == n2.agentPos;
		}
	}
	public static bool operator !=(Node n1, Node n2) {
		return !(n1 == n2);
	}

	// TODO: account for impassable tiles
	public Node affect(Character.direction dir) {
		TileData[,] newWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth];
		for(int i = 0; i < MapGene.mapHeight; i++) {
			for(int j = 0; j < MapGene.mapWidth; j++) {
				// erase the dot we just succed
				if(i == agentPos.x && j == agentPos.y)
					newWorld[i, j] = goal.world[i, j];
				else
					newWorld[i, j] = world[i, j];
				//newWorld[i, j] = world[i, j].Copy();
			}
		}
		// erase the dot we just succed
		//newWorld[agentPos[0], agentPos[1]].item = Item.ItemType.NONE;
		Vector2Int newAgentPos = Vector2Int.zero;
		switch(dir) {
			case Character.direction.RIGHT:
				newAgentPos = new Vector2Int(agentPos.x, (agentPos.y + 1) % MapGene.mapWidth);
				break;
			case Character.direction.UP:
				int newx = agentPos.x - 1;
				newAgentPos = new Vector2Int(newx < 0 ? MapGene.mapHeight + newx : newx, agentPos.y);
				break;
			case Character.direction.LEFT:
				int newy = agentPos.y - 1;
				newAgentPos = new Vector2Int(agentPos.x, newy < 0 ? MapGene.mapWidth + newy : newy);
				break;
			case Character.direction.DOWN:
				newAgentPos = new Vector2Int((agentPos.x + 1) % MapGene.mapHeight, agentPos.y);
				break;
		}
		if(!world[newAgentPos.x, newAgentPos.y].passable || Character.TheresAGhostHere(newAgentPos) && Character.charState != Character.state.REVERSE)
			return null;	// invalid action (hitting a wall)
		Node n = new Node(newWorld, newAgentPos, dir);
		n.cameFrom = this;
		n.goal = goal;
		//n.itemTiles = new List<TileData>(itemTiles.ToArray());
		n.itemTiles = new List<TileData>();
		for(int i = 0; i < itemTiles.Count; i++) {
			// copy itemTile data over, but use the tile objects from newWorld
			n.itemTiles.Add(newWorld[itemTiles[i].ij.x, itemTiles[i].ij.y]);
		}
		// remove the item at the position that the agent was
		n.itemTiles.Remove(newWorld[agentPos.x, agentPos.y]);
		return n;
	}

	public int GetActionCost(Character.direction dir) {
		if(dir == (Character.direction)(-1)) return 0;
		if(world[agentPos[0], agentPos[1]].item == Item.ItemType.NONE) {
			//Debug.Log("cat");
		} else {
			//Debug.Log("cate");
		}
		int dirtyTiles = GetItemTiles().Count - (world[agentPos[0], agentPos[1]].item != Item.ItemType.NONE ? 1 : 0);
		int turnAround = cameFrom == null ? 0 : (Character.OppositeDirection(dir, cameFrom.actionPerformed) ? 1 : 0) * 5;
		return 1 + 2 * dirtyTiles + (int)Pacman.GetDistPenalty(agentPos);// + turnAround;
	}

	public float DistBetween(TileData t1, TileData t2) {
		return Vector2Int.Distance(t1.ij, t2.ij);
	}

	public List<TileData> GetItemTiles() {
		if(itemTiles != null) return itemTiles;
		List<TileData> tiles = new List<TileData>();
		foreach(TileData t in world) {
			if(t.item != Item.ItemType.NONE) tiles.Add(t);
		}
		itemTiles = tiles;
		return tiles;
	}

	public float DistToItem() {
		List<TileData> items = GetItemTiles();
		if(items.Count == 0) return 0;
		float dist = Mathf.Infinity;
		foreach(TileData n in items) {
			float newDist = DistBetween(world[agentPos.x, agentPos.y], n);
			if(newDist < dist)
				dist = newDist;
			if(newDist <= 0)
				return newDist;
		}
		return dist;
	}
}
