using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Making sure the needed component is there
[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;

		private int _curHealth;
		public int CurHealth {
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public int damage = 25;

		public void Init() {
			CurHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats();

	// Contained here so that they can be different based on enemy strength
	// Particles
	public Transform deathParticles;
	// Camera Shake parameters
	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;

	//Sets up the death SFX that is passed onto the AudioManager
	public string deathSoundName = "Explosion";

	// Headers just write custom header text in the editor
	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	private void Start() {
		stats.Init();

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}

		//Subscribes the listed method to the delegate. Note that you msut first call the gm script
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		if (deathParticles == null) {
			Debug.LogError("No death aprticles referenced");
		}
	}

	void OnUpgradeMenuToggle(bool active) {
		// If upgrade menu is active, we don't want the enemy AI script to be active
		GetComponent<EnemyAI>().enabled = !active;
	}

	public void DamageEnemy(int damage) {
		stats.CurHealth -= damage;
		if (stats.CurHealth <= 0) {
			GameMaster.KillEnemy(this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}
	}

	private void OnCollisionEnter2D(Collision2D _colInfo) {
		Player _player = _colInfo.collider.GetComponent<Player>();
		if (_player != null) {
			_player.DamagePlayer(stats.damage);
			DamageEnemy(99999);
		}
	}

	void OnDestroy() {
		// Unsubscribes from delegate once it is destroyed, so a null object isn't referenced
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
