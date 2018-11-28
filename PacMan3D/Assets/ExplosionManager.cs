using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour {

	//private static Transform[] objects;
	private static List<Rigidbody> rbs = new List<Rigidbody>();
	private static bool wentBoom = false;
	public GameObject Characters;
	public GameObject Tiles;
	private static Vector3 origin = new Vector3(0, -.2f, 0);

	private void Reset() {
		Characters = GameObject.Find("Characters");
		Tiles = GameObject.Find("Tiles");
		AddRbs(Characters);
		AddRbs(Tiles);
	}

	public static void AddRbs(GameObject parent) {
		Transform[] objects = parent.GetComponentsInChildren<Transform>();

		foreach(Transform t in objects) {
			Rigidbody rb = t.gameObject.GetComponent<Rigidbody>();
			if(rb == null) {
				Debug.Log("adding rb to " + t.gameObject.name);
				rb = t.gameObject.AddComponent<Rigidbody>();
				rb.isKinematic = true;
			} else {
				Debug.Log("found rb on " + t.gameObject.name);
			}
			rbs.Add(rb);
		}
	}

	[ContextMenu("Go boom")]
	public void GoBoom() {
		if(!wentBoom) {
			//FindObjectOfType<Camera>().projectionMatrix = Matrix4x4.Perspective(92f, 16f / 9f, 0.01f, 1000f);	// set camera projection to perspective
			foreach(Rigidbody rb in rbs) {
				if(rb != null) {
					Character c = rb.gameObject.GetComponent<Character>();
					if(c != null) c.enabled = false;
					rb.isKinematic = false;
					Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 0.8f), Random.Range(-1f, 1f)) * 15;//(rb.transform.position - origin);
					rb.AddForceAtPosition(force * Random.Range(0.5f, 4f), origin, ForceMode.VelocityChange);
					rb.AddTorque(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * Random.Range(-300f, 360f) * 100f, ForceMode.VelocityChange);
					//rb.AddForce((rb.transform.position - origin) * 2, ForceMode.VelocityChange);
					//rb.AddExplosionForce(1000, new Vector3(0, 0, 0), 50);
				}
			}
		}
	}
}
