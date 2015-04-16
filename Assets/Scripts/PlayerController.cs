using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float zMin, zMax, xMin, xMax;
}

public class PlayerController : MonoBehaviour {
	public float speed;
	public float tilt;
	public Boundary boundary;
	public float barrelRollBonus = 1.5f;

	// For firing shots
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate = 0.5f;
	private float nextFire = 0.0f;

	// Used for double tap to do a barrel roll
	public float buttonCooldown = 0.5f; // time interval for double tap
	public float barrelRollSpeed = 100.0f;
	private int buttonCount; // keeps track of number of button taps
	private bool rotating;
	private float angleSum; // keeps track of stage of rotation
	private float tapTime;
	private float sign; // Determines direction of rotation according to button tapped

	void Start() {
		rotating = false;
		tapTime = buttonCooldown;
		buttonCount = 0;
	}

	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		}
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Rigidbody rb = GetComponent<Rigidbody> ();
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical) * speed;

		// higher speed while doing a barrel roll for fun times
		if (rotating) {
			movement *= barrelRollBonus;
		}

		rb.velocity = movement;

		// Handle out of bounds
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		// Adds a tilt to the ship when moving left to right,
		// and when not doing a barrel roll
		if (!rotating) {
			// - sign ensures that it rotates in the correct direction
			rb.rotation = Quaternion.Euler (0, 0, -tilt * moveHorizontal * speed);
		}

		// Rotates the ship by 360 when double tapped
		if (Input.GetButtonDown ("Horizontal") && !rotating) {
			if (buttonCount == 1 && tapTime > 0) {
				rotating = true;
				angleSum = 0;
				sign = Mathf.Sign(moveHorizontal);
			} else if (buttonCount < 1) {
				buttonCount++;
			} else if (buttonCount > 2) {
				buttonCount = 0;
			}
		}
		if (rotating) {
			if (Mathf.Abs(angleSum) < 360) {
				angleSum += -sign * Time.deltaTime * barrelRollSpeed;
				rb.rotation = Quaternion.Euler(0, 0, angleSum);
			} else {
				// Once rotation is done, reset all values to default
				rotating = false;
				rb.rotation.Set(0, 0, 0, 0);
				buttonCount = 0;
			}
		}

		// Update time since last tap
		if (tapTime > 0) {
			tapTime -= Time.deltaTime;
		} else {
			tapTime = buttonCooldown;
		}
	}
}
