using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGene))]
class MapGenEditor : Editor {
	public override void OnInspectorGUI() {
		MapGene ob = (MapGene)target;
		GUILayout.BeginHorizontal();
		for(int i = 0; i < ob.numMaps; i++) {
			if(GUILayout.Button("Generate\nMap " + (i + 1)))
				ob.ParseMapString(i + 1);
			if(i % 2 == 1) {
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}
		GUILayout.EndHorizontal();
		//var style = new GUIStyle(GUI.skin.button);
		//style.normal.textColor = Color.red;

		DrawDefaultInspector();
	}
}