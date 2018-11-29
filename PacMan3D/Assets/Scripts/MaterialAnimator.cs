using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour {

	public Material boxColor;
	private Color newBoxColor = Color.cyan;
	private Color newNewBoxColor = Color.white;
	private Color iBoxColor;
	[Space]
	public Material[] ghostColors;
	private Texture blueGhostTex;
	public Material blueGhost;
	private Texture[] iGhostTextures = new Texture[4];
	private Color[] iGhostColors = new Color[4];
	[Space]
	public Material unlitBlack, unlitWhite;
	private static MaterialAnimator instance;
	private Color red = new Color(0.7f, 0, 0);
	private Color blue;

	private void OnDestroy() {
		boxColor.SetColor("_EmissionColor", iBoxColor);
		for(int i = 0; i < ghostColors.Length; i++) {
			ghostColors[i].SetColor("_EmissionColor", iGhostColors[i]);
			ghostColors[i].SetTexture("_MainTex", iGhostTextures[i]);
		}
		unlitBlack.color = Color.black;
		unlitWhite.color = Color.white;
	}

	private void Start() {
		instance = this;
		iBoxColor = boxColor.GetColor("_EmissionColor");
		for(int i = 0; i < ghostColors.Length; i++) {
			iGhostColors[i] = ghostColors[i].GetColor("_EmissionColor");
			iGhostTextures[i] = ghostColors[i].GetTexture("_MainTex");
		}
		blueGhostTex = blueGhost.GetTexture("_MainTex");
		blue = blueGhost.GetColor("_EmissionColor");
	}

	// Update is called once per frame
	void Update () {
		float newValue = 0.75f + 0.5f * 0.5f * Mathf.Sin(2 * Mathf.PI / 8 * Time.time);
		newNewBoxColor.r = newValue * newBoxColor.r;
		newNewBoxColor.g = newValue * newBoxColor.g;
		newNewBoxColor.b = newValue * newBoxColor.b;
		boxColor.SetColor("_EmissionColor", newNewBoxColor);
	}

	public static void ChangeState(Character.state s) {
		if(s == Character.state.NORMAL) {
			instance.StopCoroutine("ScaryColors");
			instance.StartCoroutine("ChangeBack");
		} else {
			instance.StopCoroutine("ChangeBack");
			instance.StartCoroutine("ScaryColors");
		}
	}

	private IEnumerator ChangeBack() {
		int duration = 60;
		for(int i = 0; i < ghostColors.Length; i++) {
			ghostColors[i].SetTexture("_MainTex", iGhostTextures[i]);
		}
		for(float i = 0; i < duration; i++) {
			for(int j = 0; j < ghostColors.Length; j++) {
				//ghostColors[j].color = Color.Lerp(Color.blue, iGhostColors[j], i / (duration - 1));
				ghostColors[j].SetColor("_EmissionColor", Color.Lerp(blue, iGhostColors[j], i / (duration - 1)));
			}
			unlitBlack.color = Color.Lerp(Color.white, Color.black, i / (duration - 1));
			unlitWhite.color = Color.Lerp(Color.blue, Color.white, i / (duration - 1));

			//boxColor.color = Color.Lerp(red, iBoxColor, i / (duration - 1));
			newBoxColor = Color.Lerp(red, iBoxColor, i / (duration - 1));
			boxColor.SetColor("_EmissionColor", newBoxColor);

			yield return null;
		}
	}

	private IEnumerator ScaryColors() {
		int duration = 60;
		for(int i = 0; i < ghostColors.Length; i++) {
			ghostColors[i].SetTexture("_MainTex", blueGhostTex);
		}
		for(float i = 0; i < duration; i++) {
			for(int j = 0; j < ghostColors.Length; j++) {
				//ghostColors[j].color = Color.Lerp(iGhostColors[j], Color.blue, i / (duration - 1));
				ghostColors[j].SetColor("_EmissionColor", Color.Lerp(iGhostColors[j], blue, i / (duration - 1)));
			}
			unlitBlack.color = Color.Lerp(Color.black, Color.white, i / (duration - 1));
			unlitWhite.color = Color.Lerp(Color.white, Color.blue, i / (duration - 1));

			//boxColor.color = Color.Lerp(iBoxColor, red, i / (duration - 1));
			newBoxColor = Color.Lerp(iBoxColor, red, i / (duration - 1));
			boxColor.SetColor("_EmissionColor", newBoxColor);

			yield return null;
		}
	}
}
