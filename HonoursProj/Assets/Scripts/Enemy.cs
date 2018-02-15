using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;

		private int _curHealth;
		public int CurHealth {
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init() {
			CurHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats();

	// Headers just write text in the editor
	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	private void Start() {
		stats.Init();

		if (statusIndicator != null) {
			statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
		}
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
}
