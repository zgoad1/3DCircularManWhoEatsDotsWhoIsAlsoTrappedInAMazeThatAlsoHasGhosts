using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour {

	CharacterController cc;
	Rigidbody rb;
	Transform cam;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();
		cam = FindObjectOfType<Camera>().transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float speed = 0.3f;
		Vector3 cat = cam.forward * Input.GetAxis("Vertical") * speed + cam.right * Input.GetAxis("Horizontal") * speed;
		//cc.Move(cat * 0.25f);
		rb.MovePosition(rb.position + cat);
		transform.forward = Vector3.Lerp(transform.forward, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 0.1f);
	}
}
