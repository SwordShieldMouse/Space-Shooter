using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValues;
	public int minHazards = 1;
	public int maxHazards = 5;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText restartText;
	private bool restart;

	public GUIText gameOverText;
	private bool gameOver;

	public GUIText scoreText;
	private int score;

	void Start () {
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update() {
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}

	public void GameOver() {
		gameOverText.text = "Game Over";
		gameOver = true;
	}

	IEnumerator SpawnWaves() {
		// Gives player time to prepare before starting spawns
		yield return new WaitForSeconds (startWait);
		while (true) {
			int hazardCount = Random.Range (minHazards, maxHazards);
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (
					Random.Range (-spawnValues.x, spawnValues.x), 
					spawnValues.y, 
					spawnValues.z);
				Instantiate (hazard, spawnPosition, Quaternion.identity);
				yield return new WaitForSeconds (spawnWait); // Time delay between asteroids spawning
			}
			yield return new WaitForSeconds(waveWait);

			if (gameOver) {
				restartText.text = "Press 'R' for restart";
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore() {
		scoreText.text = "Score: " + score.ToString();
	}
}
