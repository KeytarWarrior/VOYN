using System.Collections;
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

	public IEnumerator RespawnPlayer() {
		Debug.Log("TODO: Add respawn sound");
		yield return new WaitForSeconds(2f);

		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Debug.Log("TODO: Add Spawn particles");
	}

	public static void KillPlayer(Player player) {
		Destroy(player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}
}
