using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool passable;
	public GameObject item;
	[HideInInspector] public Tile up, left, right, down;

	private void Reset() {

	}

	private void Start() {
		
	}

	/**Constructs a Tile
	 * loc - location of the Tile in 3D space
	 * passable - whether the Tile should be a floor tile or a wall
	 * item - the item to spawn on the tile, if applicable (ignored if not passable)
	 */
	public Tile(Vector3 loc, bool passable, GameObject item) {
		transform.position = loc;
		this.item = item;
	}

	/*
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		//Gizmos.DrawLine(transform.position, target.position);	// target?

		if(item.type == Item.ItemType.DOT)
			Gizmos.color = Color.red;

		Gizmos.DrawSphere(transform.position, 2);
	}
	*/

	public Tile Copy() {
		return new Tile(transform.position, passable, item);
	}
}