﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	// Same as before - failsafe to ensure we can access it but can't change it from outside
	//IMPORTANT NOTE: Static variables are not reset - theya re carried over from scene to scene
	[SerializeField]
	private int maxLives = 3;
	private static int _remainingLives;
	public static int RemainingLives {
		get { return _remainingLives; }
	}

	void Awake() {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}	
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public GameObject spawnPrefab;

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;

	private void Start() {
		if(cameraShake == null) {
			Debug.LogError("No camera shake referenced in GameMaster");
		}
		_remainingLives = maxLives;
	}

	public void EndGame() {
		Debug.Log("Game Over");
		gameOverUI.SetActive(true);
	}

	public IEnumerator _RespawnPlayer() {
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(3f);

		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy(clone, 3f);
	}

	public static void KillPlayer(Player player) {
		Destroy(player.gameObject);
		_remainingLives -= 1;
		if(_remainingLives <= 0) {
			gm.EndGame();
		} else {
			gm.StartCoroutine(gm._RespawnPlayer());
		}
	}

	public static void KillEnemy(Enemy enemy) {
		gm._KillEnemy(enemy);
	}

	// Configured so that there can be separate camera shake & death particles for each enemy
	public void _KillEnemy(Enemy _enemy) {
		// Instanced so we can destroy it after it's done
		Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone.gameObject, 4f);
		// Pulls in from outside, so we can have custom amounts for different enemies
		cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
	}
}
