using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public ItemType type;
	private float distance = 0;
	private float iy = 0;
	private Vector3 newPos = Vector3.zero;

	public enum ItemType {
		DOT, BIGDOT, FRUIT, NONE
	};

	// Use this for initialization
	void Start () {
		distance = Vector3.Distance(transform.position, Vector3.zero);
		iy = transform.position.y;
		newPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		newPos.y = iy + 0.3f * Mathf.Sin(-2 * Mathf.PI / 3 * Time.time + distance);
		transform.position = newPos;
	}
}
