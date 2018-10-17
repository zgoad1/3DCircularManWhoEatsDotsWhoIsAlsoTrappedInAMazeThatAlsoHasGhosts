﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGene : MonoBehaviour {
	
	[SerializeField] [TextArea(36, 36)] private string mapString;
	[SerializeField] [TextArea(36, 36)] private string itemString;
	[SerializeField] private TextAsset mapFile;
	[SerializeField] private TextAsset itemFile;
	[SerializeField] private float tileSize = 1.0f;
	[SerializeField] private GameObject floorTile;
	[SerializeField] private GameObject wallTile;
	[SerializeField] private GameObject dot;
	[SerializeField] private GameObject bigDot;
	[SerializeField] private GameObject fruit;
	[SerializeField] private int mapWidth, mapHeight;
	private bool inconsistentSizeError = false;

	// Executed upon clicking 'Reset' from the context menu, and upon adding the component
	private void Reset() {
		// Set default variable values
		floorTile = Resources.Load<GameObject>("floorTile");
		wallTile = Resources.Load<GameObject>("wallTile");
		mapFile = Resources.Load<TextAsset>("map");
		mapString = mapFile.text;
		itemFile = Resources.Load<TextAsset>("items");
		itemString = itemFile.text;
		dot = Resources.Load<GameObject>("dot");
		bigDot = Resources.Load<GameObject>("bigDot");
		fruit = Resources.Load<GameObject>("fruit");
		mapWidth = mapString.Split('\n')[0].Length / 2; // there's an extra char (either ' ' or '\n') for every tile; must divide by 2
		int itemWidth = mapString.Split('\n')[0].Length / 2; // there's an extra char (either ' ' or '\n') for every tile; must divide by 2
		if(itemWidth != mapWidth) inconsistentSizeError = true;
		else {
			mapHeight = 1;                                  // count an extra newline for the last line
			foreach(char c in mapString) {
				if(c == '\n') mapHeight++;                  // one tile in height for every newline in the file
			}
		}
	}

	// Use this for initialization
	void Start () {
		ParseMapString();
	}

	[ContextMenu("Generate map")]
	void ParseMapString() {
		// get an object to parent the tiles to so we don't clutter up the object list
		GameObject par = GameObject.Find("Tiles");
		if(par == null) {
			Debug.LogError("Could not find an object named 'Tiles' to parent the tile objects.");
		} else if(inconsistentSizeError) {
			Debug.LogError("Item map size is different than tile map size.");
		} else {
			// destroy all existing tiles before spawning new ones
			Tile[] toDestroy = par.GetComponentsInChildren<Tile>();
			foreach(Tile t in toDestroy) {
				Debug.Log("Destroying " + t.gameObject.name);
				DestroyImmediate(t.gameObject);
			}
			Debug.Log("======== DESTROYING FINISHED ========");

			/////////////////////////////////////////////////
			#region Parse mapString and instantiate the tiles
			/////////////////////////////////////////////////

			Vector3 tilePos = new Vector3(-mapWidth * tileSize / 2.0f, 0, mapHeight * tileSize / 2.0f);    // start creating tiles from the top left
			for(int i = 0; i < mapString.Length; i++) {
				if(mapString[i] != ' ') {  // ignore spaces
					GameObject newTile = null;
					if(mapString[i] == '+' || mapString[i] == '-' || mapString[i] == '|') {
						// create floor tile (different chars will later influence the walls around them)
						newTile = Instantiate(floorTile);
						newTile.transform.position = tilePos;
						Tile t = newTile.GetComponent<Tile>();
						GameObject itemPrefab = dot;
						// get item to spawn here based on what the item file says
						// 0 - nothing
						// 1 - big dot
						// 2 - fruit
						// anything else - dot
						switch(itemString[i]) {
							case '0':
								itemPrefab = null;
								break;
							case '1':
								itemPrefab = bigDot;
								break;
							case '2':
								itemPrefab = fruit;
								break;
							default:
								itemPrefab = dot;
								break;
						}
						if(itemPrefab != null) { 
							t.item = Instantiate(itemPrefab);
							t.item.transform.SetParent(t.transform);
							t.item.transform.localPosition = Vector3.zero;
						}
					} else if(mapString[i] == '0') {
						// create solid tile
						newTile = Instantiate(wallTile);
						newTile.transform.position = tilePos;
					} else if(mapString[i] == '\n') {
						// move tilePos all the way back to the left, and one tile's distance down
						tilePos.x -= tileSize * (mapWidth + 1);
						tilePos.z -= tileSize;
						continue;
					}
					if(newTile != null) newTile.transform.SetParent(par.transform);	// set parent
					tilePos.x += tileSize;  // move tilePos one tile's distance to the right
				}
			}

			/////////////////////////////////////////////////
			#endregion
			/////////////////////////////////////////////////
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}