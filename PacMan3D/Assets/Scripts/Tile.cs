using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool passable;
	public GameObject item = null;
	public int i, j;
	private Vector3 newScale;
	private float iy;
	private float distance;
	private float amp, ampScale = 0;
	private float per = 3;
	public static float amplitude = 0;
	[HideInInspector] public Tile up, left, right, down;

	private void Start() {
		distance = Random.Range(0f, 10f);//Vector3.Distance(transform.position, Vector3.zero);
		amp = Random.Range(0.2f, 0.8f);
		ampScale = 0;
		per = 3;
		newScale = transform.localScale;
		iy = newScale.z;
	}

	private void Update() {
		ampScale = Mathf.Lerp(ampScale, amplitude, 0.1f);
		newScale.z = iy + amp * ampScale * Mathf.Sin(-2 * Mathf.PI / per * Time.time + distance);
		transform.localScale = newScale;
	}
}