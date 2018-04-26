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

	// Particles - Contained here so that they can be different based on enemy strength
	public Transform deathParticles;
	// Camera Shake parameters
	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;

	// Variable that controls damage flash on hit
	public float damageFlashLength = 0.05f;

	public string deathSoundName = "Explosion"; //Sets up the death SFX that is passed onto the AudioManager

	public int moneyDrop = 10;	// Value of money dropped on death

	[Header("Optional: ")]  // Headers just write custom header text in the editor
	[SerializeField]
	private StatusIndicator statusIndicator;

	private void Start() {
		stats.Init();

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}

		//Subscribes the listed method to the delegate. Note that you must first call the gm script
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		if (deathParticles == null) {
			Debug.LogError("No death particles referenced");
		}
	}

	void OnUpgradeMenuToggle(bool active) {
		// If upgrade menu is active, we don't want the enemy AI script to be active
		GetComponent<EnemyAI>().enabled = !active;
	}

	public void DamageEnemy(int damage) {
		// Finds the sprite renderer used for damage flash
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.material.SetFloat("_FlashAmount", 1);
		stats.CurHealth -= damage;
		if (stats.CurHealth <= 0) {
			GameMaster.KillEnemy(this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}
		sr.material.SetFloat("_FlashAmount", 0);
	}

	public IEnumerator DamageEnemyCo (int damage) {
		// Finds the sprite renderer used for damage flash
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

		// Knock back from weapon - operates within random range for variety
		Vector2 incomingVec = new Vector2(Random.Range(150f, 400f), Random.Range(150f, 400f));
		rb.AddForce(incomingVec);

		// Damage flash
		sr.material.SetFloat("_FlashAmount", 1);
		stats.CurHealth -= damage;
		if (stats.CurHealth <= 0) {
			GameMaster.KillEnemy(this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}
		yield return new WaitForSeconds(damageFlashLength);
		sr.material.SetFloat("_FlashAmount", 0);
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
