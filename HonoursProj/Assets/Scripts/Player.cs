using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

	// Variable for falling out of bounds
	public int fallBoundary = -20;

	// Bool that keeps direction
	bool faceRight = true;

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

	// Syncs with the movement controller so we know which way player body is facing
	public void SyncDirectionFacing(bool direction) {
		faceRight = direction;
	}

	// Knock back from whatever hit - operates within random range for variety and checks for direction
	public void Knockback(bool rightDir) {
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

		// Checks which way palyer is facing - we only want a rudimentary knock back to ensure player doesn't clip evnrionment when shooting diagonally
		// A full directional one based on inverse of weapon fire vector would be messy and wouldn't serve the game's 
		if (rightDir) {
			Vector2 pushLeftVec = new Vector2(Random.Range(150f, 400f), 0);
			rb.AddForce(pushLeftVec);
		} else {
			Vector2 pusRightVec = new Vector2(Random.Range(-150f, -400f), 0);
			rb.AddForce(pusRightVec);
		}
	}
}
