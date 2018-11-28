using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExplosionManager))]
class ExplosionEditor : Editor {
	public override void OnInspectorGUI() {
		ExplosionManager ob = (ExplosionManager)target;
		var style = new GUIStyle(GUI.skin.button);
		style.normal.textColor = Color.red;

		if(GUILayout.Button("go boom", style)) {
			ob.GoBoom();
		}

		DrawDefaultInspector();
	}
}