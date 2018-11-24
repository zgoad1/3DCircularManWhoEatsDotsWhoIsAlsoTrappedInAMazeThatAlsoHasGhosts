using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : Character {
	protected direction where = direction.DOWN;
	private Node start, goal;
	private static direction[] actions = new direction[] { direction.DOWN, direction.LEFT, direction.RIGHT, direction.UP };


	protected override void Start() {
		base.Start();
		TileData[,] startWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth], goalWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth];
		for(int i = 0; i < MapGene.mapHeight; i++) {
			for(int j = 0; j < MapGene.mapWidth; j++) {
				Item.ItemType t = map.tileMap[i, j].item == null ? Item.ItemType.NONE : map.tileMap[i, j].item.GetComponent<Item>().type;
				startWorld[i, j] = new TileData(t, new Vector2Int(map.tileMap[i, j].i, map.tileMap[i, j].j));
				goalWorld[i, j] = new TileData(Item.ItemType.NONE, new Vector2Int(map.tileMap[i, j].i, map.tileMap[i, j].j));
			}
		}
		Vector3 normCoords = GetNormalizedCoords(rb.position);
		Vector2Int coords = new Vector2Int((int)normCoords.z, (int)normCoords.x);
		start = new Node(startWorld, coords, where);
		goal = new Node(goalWorld, coords, where);
		Astar();
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
		Astar();
		base.ReachTile();
	}

	protected float Heuristic(float d) {
		return d;
	}

	private Node GetMinFscore(List<Node> list) {
		Node minfscore = list[0];
		foreach(Node n in list) {
			if(n.fscore < minfscore.fscore) minfscore = n;
		}
		return minfscore;
	}

	private Node GetNextNode(Node c) {
		if(c.cameFrom == null) return c;
		Node next = c, prev = null;
		while(next != null) {
			prev = next;
			next = next.cameFrom;
		}
		return prev;
	}

	protected void Astar() {
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>();
		openSet.Add(start);
		start.gscore = 0;
		start.fscore = Heuristic(start.distToDirty());
		while(openSet.Count > 0) {
			Node current = GetMinFscore(openSet);
			if(current == goal) {
				where = GetNextNode(current).actionPerformed;
				return;
			}
			openSet.Remove(current);
			closedSet.Add(current);

			current.neighbors = new List<Node>();
			foreach(direction a in actions) {
				current.neighbors.Add(current.affect(a));
			}

			foreach(Node n in current.neighbors) {
				if(closedSet.Contains(n)) continue;
				float tempgscore = current.gscore + current.getActionCost(n.actionPerformed);
				if(!openSet.Contains(n)) {
					openSet.Add(n);
				} else if(tempgscore >= n.gscore) {
					continue;
				}

				n.cameFrom = current;
				n.gscore = tempgscore;
				n.fscore = n.gscore + Heuristic(n.distToDirty());
			}
		}
	}
}

class TileData {
	public Item.ItemType item;
	public Vector2Int ij;

	public TileData(Item.ItemType item, Vector2Int coords) {
		this.item = item;
		ij = coords;
	}
	public TileData(Item.ItemType item, int i, int j) {
		this.item = item;
		ij = new Vector2Int(i, j);
	}

	public static bool operator ==(TileData thys, TileData that) {
		return thys.item == that.item && thys.ij == that.ij;
	}
	public static bool operator !=(TileData thys, TileData that) {
		return !(thys.item == that.item && thys.ij == that.ij);
	}
}

class Node {
	public float fscore = Mathf.Infinity, gscore = Mathf.Infinity;
	public List<Node> neighbors;
	public TileData[,] world = new TileData[MapGene.mapHeight, MapGene.mapWidth];
	public Vector2Int agentPos;
	public Character.direction actionPerformed;
	public Node cameFrom;

	List<TileData> dirtyTiles;


	public Node(TileData[,] world, Vector2Int agentPos, Character.direction action) {
		this.world = world;
		this.agentPos = agentPos;
		actionPerformed = action;
	}

	public static bool operator ==(Node n1, Node n2) {
		// this try-catch is equivalent to if(n2 == false), which we can't use for obvious reasons
		if(!(n2 is Node)) {
			Debug.Log("n2 is null. n1: " + n1);
		}
		if(n1 is Node ^ n2 is Node) {
			Debug.Log("this");
			return false;
		}
		if(n1.GetItemTiles().Count == 0 && n2.GetItemTiles().Count == 0) return true;
		for(int i = 0; i < MapGene.mapHeight; i++) {
			for(int j = 0; j < MapGene.mapWidth; j++) {
				if(n1.world[i, j].item != n2.world[i, j].item) return false;
			}
		}
		return n1.agentPos == n2.agentPos;
	}
	public static bool operator !=(Node n1, Node n2) {
		return !(n1 == n2);
	}

	// TODO: account for impassable tiles
	public Node affect(Character.direction dir) {
		TileData[,] newWorld = new TileData[MapGene.mapHeight, MapGene.mapWidth];
		System.Array.Copy(world, newWorld, MapGene.mapHeight * MapGene.mapWidth);
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
		return new Node(newWorld, newAgentPos, dir);
	}

	public int getActionCost(Character.direction dir) {
		if(dir == (Character.direction)(-1)) return 0;
		return 1 + 2 * (GetItemTiles().Count - (world[agentPos[0],agentPos[1]].item != Item.ItemType.NONE ? 1 : 0));
	}

	public float distBetween(TileData t1, TileData t2) {
		return Vector2Int.Distance(t1.ij, t2.ij);
	}

	public List<TileData> GetItemTiles() {
		if(dirtyTiles != null) return dirtyTiles;
		List<TileData> tiles = new List<TileData>();

		dirtyTiles = tiles;
		return tiles;
	}

	public float distToDirty() {
		List<TileData> items = GetItemTiles();
		if(items.Count == 0) return 0;
		float dist = Mathf.Infinity;
		foreach(TileData n in items) {
			float newDist = distBetween(world[agentPos.x, agentPos.y], n);
			if(newDist < dist) dist = newDist;
		}
		return dist;
	}
}
