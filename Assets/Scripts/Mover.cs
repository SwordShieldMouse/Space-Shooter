using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed;
	public GameObject player;

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		player = GameObject.Find ("Player");

		// Shots move relative to the ship, so they can curve
		rb.velocity = transform.forward * speed + player.GetComponent<Rigidbody>().velocity;
	}
}
