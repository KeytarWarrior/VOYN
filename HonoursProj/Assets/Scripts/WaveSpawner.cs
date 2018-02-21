using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

	// Defining enum for states
	public enum SpawnState { SPAWNING, WAITING, COUNTING};

	[System.Serializable] // Allows us to change value of the instances within the inspector
	public class Wave {
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave[] waves;
	private int nextWave = 0; // Index of wave
	// Getter for NextWave. Good coding practice.
	public int NextWave {
		get { return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;

	private float waveCountdown;
	// Getter to retrieve WaveCountdown - again good coding practice
	public float WaveCountdown {
		get { return waveCountdown + 1f; }
	}

	// Timer inbetween enemy search for performance reasons
	private float searchTimer = 1f;
	
	// Initialize the enum and default it to COUNTING
	private SpawnState state = SpawnState.COUNTING;
	// Getter so that we can access the SpawnState without making it public
	public SpawnState State {
		get { return state; }
	}

	// Use this for initialization
	void Start () {
		// Quick safety check for Spawn Points
		if (spawnPoints.Length == 0) {
			Debug.LogError("No spawn points referenced.");
		}

		waveCountdown = timeBetweenWaves;
	}
	
	// Update is called once per frame
	void Update () {
		// Checks if player has killed off all enemies before proceeding with next wave
		if (state == SpawnState.WAITING) {
			if (!EnemyIsAlive()) {
				// Begin a new round
				WaveCompleted();
			} else {
				return;
			}
		}

		if (waveCountdown <= 0) {
			if (state != SpawnState.SPAWNING) {
				StartCoroutine(SpawnWave(waves[nextWave]));
			} 
		} else {
			// Makes sure we go down the appropriate amount of time for each frame. Framerate independent.
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted() {
		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		// Safety check so we don't go over the max defined wave
		if (nextWave + 1 > waves.Length - 1) {
			nextWave = 0;
			Debug.Log("All waves complete! Looping...");
		} else {
			nextWave++;
		}
	}

	bool EnemyIsAlive() {
		// Goes through search timer for perfomance reasons
		searchTimer -= Time.deltaTime;
		if(searchTimer <= 0f) {
			searchTimer = 1f;
			// If no GameObject tagged as enemy exists, return false
			// This action is very performance intensive so a timer is implemented
			if (GameObject.FindGameObjectWithTag("Enemy") == null) {
				return false;
			}
		}
		return true;
	}

	// Using Ienumerator so that we can wait a set amount of time within the method
	IEnumerator SpawnWave (Wave _wave) {
		Debug.Log("Spawning Wave " + _wave.name);
		state = SpawnState.SPAWNING; // Now spawning

		// Spawns appropriate number of enemies and waits inbetween according to set rate
		for (int i = 0; i < _wave.count; i++) {
			SpawnEnemy(_wave.enemy);
			// Because of IEnum we can wait for a bit before continuing with loop
			yield return new WaitForSeconds(1f / _wave.rate);
		}

		state = SpawnState.WAITING; // Waiting for palyer to kill wave

		yield break;
	}

	// Spawns enemies
	void SpawnEnemy (Transform _enemy) {
		Debug.Log("Spawning enemy " + _enemy.name);

		// Picks between a random spawn point
		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}

}
