using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats {
		public int maxHealth = 100;

		private int _curHealth;
		public int curHealth {
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
		}

		public void Init() {
			curHealth = maxHealth;
		}
	}

	public PlayerStats stats = new PlayerStats();

	// Variable for falling out of bounds
	public int fallBoundary = -20;

	// SFX variables
	public string deathSoundName = "DeathVoice";
	public string damageSoundName = "Grunt";

	private AudioManager audioManager;

	[SerializeField]
	private StatusIndicator statusIndicator;

	private void Start() {
		stats.Init();

		if (statusIndicator == null) {
			Debug.LogError("No status indicator referenced on Player");
		} else {
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}

		//Subscribes the listed method to the delegate. Note that you msut first call the gm script
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		audioManager = AudioManager.audioManInstance;
		if(audioManager == null) {
			Debug.LogError("No AudioManager in scene.");
		}
	}

	public void Update() {
		if (transform.position.y <= fallBoundary) {
			DamagePlayer(999);
		}
	}

	void OnUpgradeMenuToggle(bool active) {
		// If upgrade menu is active, we don't want the movement script to be active
		GetComponent<Platformer2DUserControl>().enabled = !active;
		// Same with weapon script - find it as a child and deactivate/activate it
		Weapon _weapon = GetComponentInChildren<Weapon>();
		if (_weapon != null)
			_weapon.enabled = !active;
	}

	void OnDestroy() {
		// Unsubscribes from delegate once it is destroyed, so a null object isn't referenced
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}

	public void DamagePlayer (int damage) {
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {
			// Play death SFX
			audioManager.PlaySound(deathSoundName);

			// Kill palyerr
			GameMaster.KillPlayer(this);
		} else {
			// Play damage SFX
			audioManager.PlaySound(damageSoundName);
		}

		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
	}

}
