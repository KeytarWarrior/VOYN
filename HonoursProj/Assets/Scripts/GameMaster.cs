using System.Collections;
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

	// Prefabs
	public Transform playerPrefab;
	public Transform spawnPoint;
	public GameObject spawnPrefab;

	// SFX
	public string respawnCountdownSoundName = "RespawnCountdown";
	public string spawnSoundName = "Spawn";
	public string gameOverSoundName = "GameOver";

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;

	// cache
	private AudioManager audioManager;

	private void Start() {
		if(cameraShake == null) {
			Debug.LogError("No camera shake referenced in GameMaster");
		}
		_remainingLives = maxLives;

		// caching
		audioManager = AudioManager.audioManInstance;
		if (audioManager == null) {
			Debug.LogError("No AudioManager found in the scene.");
		}
	}

	public void EndGame() {
		audioManager.PlaySound(gameOverSoundName);

		Debug.Log("Game Over");
		gameOverUI.SetActive(true);
	}

	public IEnumerator _RespawnPlayer() {
		// Uses the new AudioManager script logic to play sound - much cleaner & more modular
		audioManager.PlaySound(respawnCountdownSoundName);
		yield return new WaitForSeconds(3f);

		audioManager.PlaySound(spawnSoundName);

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
		// Death Sound - Modular so can eb setup for different enemies
		audioManager.PlaySound(_enemy.deathSoundName);
		
		// Adding Particles - Instanced so we can destroy it after it's done 
		Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone.gameObject, 4f);

		// Camera Shake - Pulls in from outside, so we can have custom amounts for different enemies
		cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
	}
}
