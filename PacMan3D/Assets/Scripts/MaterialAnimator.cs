using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour {

	public Material boxColor;
	private Color newColor;

	// Update is called once per frame
	void Update () {
		float newValue = 1.4f + 0.8f * 0.5f * Mathf.Sin(2 * Mathf.PI / 8 * Time.time);
		newColor.g = newValue;
		newColor.b = newValue;
		boxColor.SetColor("_EmissionColor", newColor);
	}
}
