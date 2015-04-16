using UnityEngine;
using System.Collections;

public class AsteroidMover : MonoBehaviour {
	public float speed;
	public float tilt;

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (Random.Range(-tilt, tilt), 0, -1 * speed);
	}
}
