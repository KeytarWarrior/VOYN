﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Start() {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}	
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public GameObject spawnPrefab;

	public IEnumerator RespawnPlayer() {
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(3f);

		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy(clone, 3f);
	}

	public static void KillPlayer(Player player) {
		Destroy(player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}

	public static void KillEnemy(Enemy enemy) {
		Destroy(enemy.gameObject);
	}
}
