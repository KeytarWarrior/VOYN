using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	// Same as before - failsafe to ensure we can access it but can't change it from outside
	//IMPORTANT NOTE: Static variables are not reset - theya re carried over from scene to scene
	[SerializeField] private int maxLives = 3;
	private static int _remainingLives;
	public static int RemainingLives {
		get { return _remainingLives; }
	}

	// Currency initialization
	[SerializeField] private int startingMoney;
	public static int Money;

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

	[SerializeField] private GameObject gameOverUI;
	[SerializeField] private GameObject upgradeMenu;
	[SerializeField] private GameObject levelMenu;
	[SerializeField] private WaveSpawner waveSpawner;

	// Delegate, so that we can pause certain functions while upgrade menu is up, without pausing the entire game
	// Creates a type that can strore a bunch of references to functions. 
	// Can be invoked to call all the registered to it functions.
	public delegate void UpgradeMenuCallback(bool active);
	// Instance of the above type
	public UpgradeMenuCallback onToggleUpgradeMenu;

	// Cache
	private AudioManager audioManager;

	private void Start() {
		if(cameraShake == null) {
			Debug.LogError("No camera shake referenced in GameMaster");
		}
		_remainingLives = maxLives;

		// Money
		Money = startingMoney;

		// Caching
		audioManager = AudioManager.audioManInstance;
		if (audioManager == null) {
			Debug.LogError("No AudioManager found in the scene.");
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			ToggleUpgradeMenu();
		} else if (Input.GetKeyDown(KeyCode.L)) {
			ToggleLevelMenu();
		} else if (Input.GetKeyDown(KeyCode.R)) {
				SceneManager.LoadScene("MainMenu");
		}
	}

	private void ToggleUpgradeMenu() {	
		upgradeMenu.SetActive(!upgradeMenu.activeSelf); // Sets the state to the opposite of what it currently is - better than hard coding it to on or off		
		waveSpawner.enabled = !upgradeMenu.activeSelf; // Makes sure that if the upgrade menu is enabled, the enemy spawner is disabled
		onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf); // Invokes all methods subscribed to this event
	}

	private void ToggleLevelMenu() {
		levelMenu.SetActive(!levelMenu.activeSelf); // Sets the state to the opposite of what it currently is - better than hard coding it to on or off		
		waveSpawner.enabled = !levelMenu.activeSelf; // Makes sure that if the upgrade menu is enabled, the enemy spawner is disabled
	}

	public void EndGame() {
		audioManager.PlaySound(gameOverSoundName);

		Debug.Log("Game Over");
		gameOverUI.SetActive(true);
	}

	public IEnumerator _RespawnPlayer() {	
		audioManager.PlaySound(respawnCountdownSoundName);  // Uses the new AudioManager script logic to play sound - much cleaner & more modular
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
		audioManager.PlaySound(_enemy.deathSoundName);  // Death Sound - Modular so can be setup for different enemies

		Money += _enemy.moneyDrop;  // Adds the Money dropped from enemy to resource. Modular so can be different for different enemy types.
		audioManager.PlaySound("Money");

		// Adding Particles - Instanced so we can destroy it after it's done 
		Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone.gameObject, 4f);

		// Camera Shake - Pulls in from outside, so we can have custom amounts for different enemies
		cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
	}
}
