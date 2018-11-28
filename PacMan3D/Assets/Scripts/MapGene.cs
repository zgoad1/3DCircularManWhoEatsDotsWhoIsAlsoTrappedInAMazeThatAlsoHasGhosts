using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGene : MonoBehaviour {

	public int numMaps = 3;
	public int mapIndex = 1;
	[SerializeField] [TextArea(36, 36)] private string mapString;
	[SerializeField] [TextArea(36, 36)] private string itemString;
	[SerializeField] private TextAsset mapFile;
	[SerializeField] private TextAsset itemFile;
	public float tileSize = 1.0f;
	[SerializeField] private GameObject floorTile;
	[SerializeField] private GameObject wallTile;
	[SerializeField] private GameObject dot;
	[SerializeField] private GameObject bigDot;
	[SerializeField] private GameObject fruit;
	public int mapW, mapH;
	public static int mapWidth, mapHeight;
	[HideInInspector] public Tile[,] tileMap;
	private bool inconsistentSizeError = false;

	// Executed upon clicking 'Reset' from the context menu, and upon adding the component
	private void Reset() {
		// Set default variable values
		floorTile = Resources.Load<GameObject>("floorTile");
		wallTile = Resources.Load<GameObject>("wallTile");
		mapFile = Resources.Load<TextAsset>("map1");
		itemFile = Resources.Load<TextAsset>("items1");
		dot = Resources.Load<GameObject>("dot");
		bigDot = Resources.Load<GameObject>("bigDot");
		fruit = Resources.Load<GameObject>("fruit");
	}
	
	public void SetMapStrings(int mapIndex) {
		mapFile = Resources.Load<TextAsset>("map" + mapIndex);
		itemFile = Resources.Load<TextAsset>("items" + mapIndex);
		mapString = mapFile.text;
		itemString = itemFile.text;
		string[] mapArray = mapString.Split('\n');
		mapW = (mapArray[0].Length + 1) / 2; // there's an extra ' ' for every tile and it doesn't count the '\n'; must add 1 and divide by 2
		mapH = mapArray.Length;
		mapWidth = mapW;
		mapHeight = mapH;
		Debug.Log("Map width: " + mapW + "\nMap height: " + mapH);
		int itemWidth = mapString.Split('\n')[0].Length / 2;
		if(itemWidth != mapW) inconsistentSizeError = true;
	}

	// Use this for initialization
	void Awake () {
		mapWidth = mapW;
		mapHeight = mapH;
		ParseMapString(mapIndex);
		ExplosionManager.AddRbs(FindObjectOfType<ExplosionManager>().Characters);
		ExplosionManager.AddRbs(FindObjectOfType<ExplosionManager>().Tiles);
	}
	
	public void ParseMapString(int mapIndex) {
		this.mapIndex = mapIndex;
		SetMapStrings(mapIndex);
		string[] mapArray = mapString.Split('\n');
		mapW = (mapArray[0].Length + 1) / 2; // there's an extra ' ' for every tile and it doesn't count the '\n'; must add 1 and divide by 2
		Debug.Log("maparray[0]: " + mapArray[0] + "\nlength: " + mapArray[0].Length);
		Debug.Log("maparray[1]: " + mapArray[1] + "\nlength: " + mapArray[1].Length);
		mapH = mapArray.Length;
		mapWidth = mapW;
		mapHeight = mapH;
		Debug.Log("Map width: " + mapW + "\nMap height: " + mapH);
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
				//Debug.Log("Destroying " + t.gameObject.name);
				DestroyImmediate(t.gameObject);
			}
			//Debug.Log("======== DESTROYING FINISHED ========");

			/////////////////////////////////////////////////
			#region Parse mapString and instantiate the tiles
			/////////////////////////////////////////////////

			tileMap = new Tile[mapH, mapW];
			int ti = 0, tj = 0;
			Vector3 tilePos = new Vector3(-mapW * tileSize / 2.0f + 0.5f, 0, mapH * tileSize / 2.0f - 0.5f);    // start creating tiles from the top left
			for(int i = 0; i < mapString.Length; i++) {
				if(mapString[i] != ' ' && mapString[i] != '\0') {  // ignore spaces and null chars
					//Debug.Log("mapString: '" + mapString[i] + "'\niteString: '" + itemString[i] + "'");
					GameObject newTile = null;
					if(mapString[i] == '+' || mapString[i] == '-' || mapString[i] == '|') {
						// create floor tile (different chars will later influence the walls around them)
						newTile = Instantiate(floorTile);
						newTile.transform.position = tilePos;
						newTile.name = string.Format("floor_{0},{1}", ti, tj);
						Tile t = newTile.GetComponent<Tile>();
						t.i = ti;
						t.j = tj;
						tileMap[ti, tj] = t;
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
								//Debug.Log("FOUSANE  AD OT1@!! CUZE FO '" + itemString[i] + "'");
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
						newTile.name = string.Format("wall_{0},{1}", ti, tj);
						tileMap[ti, tj] = newTile.GetComponent<Tile>();
						tileMap[ti, tj].i = ti;
						tileMap[ti, tj].j = tj;
					} else if(mapString[i] == '\n') {
						// move tilePos all the way back to the left, and one tile's distance down
						tilePos.x -= tileSize * (mapW + 1);
						tilePos.z -= tileSize;
						tj = 0;
						ti++;
						continue;
					}
					if(newTile != null) {
						newTile.transform.SetParent(par.transform); // set parent
						tj++;
					} else {
						Debug.LogWarning("Unknown symbol found in map file: '" + mapString[i] + "'\n(if the quotes are empty it's probably ok i think)");
					}
					tilePos.x += tileSize;  // move tilePos one tile's distance to the right
				}
			}

			// Set up, down, left, and right of each tile
			for(int i = 0; i < mapH; i++) {
				for(int j = 0; j < mapW; j++) {
					tileMap[i, j].left = j == 0 ? tileMap[i, mapW - 1] : tileMap[i, j - 1];
					tileMap[i, j].right = tileMap[i, (j + 1) % mapW];
					tileMap[i, j].up = i == 0 ? tileMap[mapH - 1, j] : tileMap[i - 1, j];
					tileMap[i, j].down = tileMap[(i + 1) % mapH, j];
					//Debug.Log(string.Format("left: {0}, right: {1}, up: {2}, down: {3}", tileMap[i, j].left.gameObject.name, tileMap[i, j].right.gameObject.name, tileMap[i, j].up.gameObject.name, tileMap[i, j].down.gameObject.name));
				}
			}

			/////////////////////////////////////////////////
			#endregion
			/////////////////////////////////////////////////
			
		}
	}
}
