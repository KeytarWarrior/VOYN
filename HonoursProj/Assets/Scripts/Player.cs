using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

	// Variable for falling out of bounds
	public int fallBoundary = -20;

	// SFX variables
	public string deathSoundName = "DeathVoice";
	public string damageSoundName = "Grunt";

	private AudioManager audioManager;

	[SerializeField]
	private StatusIndicator statusIndicator;

	// Casting the instance into a local variable so it looks neater
	private PlayerStats stats;

	private void Start() {

		stats = PlayerStats.instance;

		// Safety check to make sure palyer health has been reset properly
		stats.curHealth = stats.maxHealth;

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


		// Calls on the health regen rate method
		InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
	}

	// Health regeneration logic
	void RegenHealth() {
		// Increments health and updates the status indicator
		stats.curHealth += 1;
		statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
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
